using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using CustomPathfinding;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingManager : MonoBehaviour
    {
        public enum Pathfinder
        {
            AStar,
            BFS
        }
        
        private const int RESULTS_QUEUE_CAPACITY = 64;
        private readonly Queue<PathResult> results = new Queue<PathResult>(RESULTS_QUEUE_CAPACITY);
        [Range(1,4)]
        public int MaxThreadCount;
        
        private PathfindingGrid _pathfindingGraph;
        private WaitForSeconds _wfs;
        private Thread[] _threads;

        private void Awake()
        {
           _pathfindingGraph = GetComponent<PathfindingGrid>();
            _wfs = new WaitForSeconds(0.001f);
            ThreadPool.SetMaxThreads(MaxThreadCount, MaxThreadCount);
        }

        private void Start()
        {
            StartCoroutine(ReadPathfindingResults());  
        }

        private IEnumerator ReadPathfindingResults()
        {
            while (true)
            {
                yield return _wfs;

                lock (results)
                {
                    //read only in case that we have a result queued up.
                    if (results.Count > 0)
                    {
                        PathResult result = results.Dequeue();
                        
                        if(PathfindingAlgorithms.isDebugMode)
                            //AStar.DebugPath(result.path);
                        
                        result.callback(result.path, result.success);

                    }
                }
            }


        }

        /* Non multithreading version
        public Vector3[] RequestPath(Vector3 source, Vector3 target, float agentRadius)
        {
            Node sourceNode = _pathfindingGraph.GetNodeFromWorldPosition(source);
            Node targetNode = _pathfindingGraph.GetNodeFromWorldPosition(target);

            if (!targetNode.Walkable)
            {
                Debug.LogError("No se puede llegar hasta el nodo indicado");
                return null;
            }
            
            _pathfindingGraph.AStar.AStarSearch(_pathfindingGraph,sourceNode,targetNode);
            
            return _pathfindingGraph.SmoothPath(_pathfindingGraph.ReconstructPath(targetNode), agentRadius);
        }*/


        public void FinishedProcessingPath(PathResult result)
        {
            lock (results)
            {
                results.Enqueue(result);
            }
        }
        
        public void RequestPath(PathRequest request, Pathfinder pathfinder)
        {
            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                switch (pathfinder)
                {
                        case Pathfinder.BFS:
                            //todo
                            PathfindingAlgorithms.BFS(_pathfindingGraph,
                                _pathfindingGraph.GetNodeFromWorldPosition(request.PathStart), int.MaxValue);
                            break;
                        
                        case Pathfinder.AStar:
                            PathfindingAlgorithms.AStarSearch(_pathfindingGraph, request, FinishedProcessingPath);
                            break;
                }
            });
        }

        public struct PathResult
        {
            public Vector3[] path;
            public bool success;
            public int ThreadId;
            public Action<Vector3[], bool> callback;

            public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback, int threadId)
            {
                this.path = path;
                this.success = success;
                this.callback = callback;
                this.ThreadId = threadId;
            }
        }
        
        public struct PathRequest
        {
            public Vector3 PathStart;
            public Vector3 PathEnd;
            public readonly Action<Vector3[], bool> Callback;
            public readonly float AgentRadius;

            public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback, float agentRadius)
            {
                this.PathStart = pathStart;
                this.PathEnd = pathEnd;
                this.Callback = callback;
                this.AgentRadius = agentRadius;
            }
        }
    }
}
