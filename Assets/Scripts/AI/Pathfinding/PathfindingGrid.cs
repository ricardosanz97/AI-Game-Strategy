using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace CustomPathfinding
{
	
	public class PathfindingGrid : MonoBehaviour,IWeightedGraph
	{
		[Header("Grid Properties")] 
		public Vector2 GridWorldSize;
		[Tooltip("if this is false, the bottom right will be usd instead.")]
		public bool UseTransformAsGridOrigin;
		[SerializeField] 
		private LayerMask _unwalkableMask;
		[Header("Debug Properties")] 
		public Color WalkableColor;
		public Color UnWalkableColor;
		[Range(0.1f,1f)]
		public float ScaleFactor = 1f;
		public Node NodePrefab;
		
		private float _nodeDiameter;
		public Node[,] Grid { get; private set; }
		public int GridSizeX { get; private set; }
		public int GridSizeZ { get; private set; }
		private GameObject nodeContainer;
		
		public int NodeCount => GridSizeX * GridSizeZ;

		void Start ()
		{
			InitializePathfindingGrid();
		}

		public void InitializePathfindingGrid()
		{
			_nodeDiameter = NodePrefab.NodeRadius * 2;
			GridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
			GridSizeZ = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
			
			CreateGrid();
		}

		public void CreateGrid()
		{
			if (nodeContainer != null)
			{
				Destroy(nodeContainer);
			}
			
			nodeContainer = new GameObject("Node Container");
			nodeContainer.transform.SetParent(this.transform);
			Grid = new Node[GridSizeX, GridSizeZ];
			Vector3 gridOrigin;
			//Collider[] results = new Collider[16];
			
			if(UseTransformAsGridOrigin)
				gridOrigin = transform.position;
			else
				gridOrigin = transform.position - Vector3.right * GridWorldSize.x/2 - Vector3.forward * GridWorldSize.y/2 ;
			
			Debug.DrawRay(Vector3.zero, gridOrigin);
			for (int i = 0; i < GridSizeX; i++)
			{
				for (int j = 0; j < GridSizeZ; j++)
				{
					Node.ENodeType nodeType = Node.ENodeType.Walkable;
					Vector3 nodeWorldPosition = gridOrigin + Vector3.right * (i * _nodeDiameter + NodePrefab.NodeRadius) +
					                            Vector3.forward * (j * _nodeDiameter + NodePrefab.NodeRadius);

					/*results = new Collider[16];
					Physics.OverlapBoxNonAlloc(nodeWorldPosition,
						new Vector3(NodeRadius, NodeRadius, NodeRadius), results,Quaternion.identity);*/

					if (Physics.CheckBox(nodeWorldPosition, new Vector3(NodePrefab.NodeRadius,NodePrefab.NodeRadius,NodePrefab.NodeRadius),Quaternion.identity, _unwalkableMask))
						nodeType = Node.ENodeType.NonWalkable;

					InitializeNode(i,j,nodeWorldPosition,nodeType);
				}
			}
		}

		private void InitializeNode(int i, int j, Vector3 nodeWorldPosition, Node.ENodeType nodeType)
		{
			Grid[i,j] = Instantiate(NodePrefab, nodeWorldPosition, Quaternion.identity, nodeContainer.transform);
			Grid[i, j].NodeType = nodeType;

			if (nodeType == Node.ENodeType.NonWalkable)
			{
				Material mat = Grid[i, j].GetComponent<MeshRenderer>().material;
				mat.color = new Color(0.1f, 0, 0, 0.1f);
				Grid[i, j].GetComponent<MeshRenderer>().material = mat;
			}
			else
			{
				Material mat = Grid[i, j].GetComponent<MeshRenderer>().material;
				mat.color = new Color(0,0.2f,0,0.1f);
				Grid[i, j].GetComponent<MeshRenderer>().material = mat;
			}

			Grid[i, j].transform.localScale *= ScaleFactor;
			
			Grid[i, j].GridX = i;
			Grid[i, j].GridZ = j;
		}

		public Node GetNodeFromWorldPosition(Vector3 worldPosition)
		{
			float percentX = (worldPosition.x + GridWorldSize.x/2) / GridWorldSize.x;
			float percentY = (worldPosition.z + GridWorldSize.y/2) / GridWorldSize.y;
			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);

			int x = Mathf.RoundToInt((GridSizeX-1) * percentX);
			int y = Mathf.RoundToInt((GridSizeZ-1) * percentY);

			return Grid[x,y];
		}
		
		public IEnumerable<Node> GetNeighbors(Node currentNode)
		{	
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
				    if(i == 0 && j == 0) continue;
					
					var x = currentNode.GridX + i;
					var z = currentNode.GridZ + j;

					if ((x >= 0 && x < GridSizeX) && (z >= 0 && z < GridSizeZ) && (Grid[x,z].NodeType == Node.ENodeType.Walkable ||  Grid[x,z].NodeType == Node.ENodeType.NextToWall))
					{
						yield return Grid[currentNode.GridX + i, currentNode.GridZ + j];
					}
				}
			}
		}
		
		public IEnumerable<Node> GetNeighborsWithObstacles(Node currentNode)
		{	
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if(i == 0 && j == 0) continue;
					
					var x = currentNode.GridX + i;
					var z = currentNode.GridZ + j;

					if ((x >= 0 && x < GridSizeX) && (z >= 0 && z < GridSizeZ) && Grid[x,z].NodeType != Node.ENodeType.Invisible)
					{
						yield return Grid[currentNode.GridX + i, currentNode.GridZ + j];
					}
				}
			}
		}

		//it gives as the cost of the edge between these two nodes
		public float Cost(Node currentNode, Node neighbor)
		{	
			if (currentNode.GridX == neighbor.GridX || currentNode.GridZ == neighbor.GridZ && neighbor.NodeType == Node.ENodeType.Walkable)
				return 1f;

			if (!(currentNode.GridX == neighbor.GridX || currentNode.GridZ == neighbor.GridZ) &&
			    neighbor.NodeType == Node.ENodeType.Walkable)
				return 1.4f;
			

			return int.MaxValue;

		}

		public Vector3[] SmoothPath(Vector3[] pathToSmooth, float agentRadius)
		{
			LinkedList<Vector3> smoothedPath = new LinkedList<Vector3>(pathToSmooth);
			
			LinkedListNode<Vector3> startingPoint = smoothedPath.First;
			LinkedListNode<Vector3> current = startingPoint.Next;
			
			while (current != null && current.Next != null)
			{
				if (IsWalkable(GetNodeFromWorldPosition(startingPoint.Value), GetNodeFromWorldPosition(current.Next.Value), agentRadius))
				{
					var temp = current;
					current = current.Next;
					smoothedPath.Remove(temp);
				}
				else
				{
					startingPoint = current;
					current = current.Next;
				}
					
			}
			
			return smoothedPath.ToArray();
		}
		
		private bool IsWalkable(Node n1, Node n2, float agentRadius)
		{
			Vector3 dir = n2.WorldPosition - n1.WorldPosition;

			for (float i = 0; i < 1; i += NodePrefab.NodeRadius/5.0f)
			{
				Vector3 samplePoint = n1.WorldPosition + i * dir;

				Vector3 rightSampledPoint = samplePoint + new Vector3(agentRadius,0,0);
				if (GetNodeFromWorldPosition(rightSampledPoint).NodeType != Node.ENodeType.Walkable) return false;
				
				Vector3 bottomSamplePoint = samplePoint + new Vector3(0,0,- agentRadius);
				if (GetNodeFromWorldPosition(bottomSamplePoint).NodeType != Node.ENodeType.Walkable) return false;
				
				Vector3 topSampledPoint = samplePoint + new Vector3(0,0,agentRadius);
				if (GetNodeFromWorldPosition(topSampledPoint).NodeType != Node.ENodeType.Walkable) return false;
				
				Vector3 leftSampledPoint = samplePoint + new Vector3(- agentRadius,0,0);
				if (GetNodeFromWorldPosition(leftSampledPoint).NodeType != Node.ENodeType.Walkable) return false;
				

			}
			return true;
		}
	}
	
	

}
