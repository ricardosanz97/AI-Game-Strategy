using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move : Action
{
    public int maxMoves;
    public bool moving = false;
    public CellBehaviour OnGoingCell;
    public bool PathReceived = false;
    public Vector3[] path;
    private int counter = 0;
    public override void Act()
    {
        if (!PathReceived)
        {
            counter = 0;
            return;
        }
        moving = true;
        //ya tenemos destino
        Vector3 initial = GetComponent<Troop>().gameObject.transform.position;
        Debug.Log("request entre " + initial + " y " + OnGoingCell.PNode.gameObject.transform.position);
        GetComponent<Troop>()._pathfindingManager.RequestPath(new Pathfinding.PathfindingManager.PathRequest(initial, OnGoingCell.PNode.gameObject.transform.position, PathReceiver, 0.5f),false);
        PathReceived = false;
        OnGoingCell = null;
    }

    private void PathReceiver(Vector3[] path, bool isPossible)
    {
        if (isPossible)
        {
            this.path = path;
            Debug.Log("Se ha generado el camino y tiene " + path.Length + " nodos. ");
            StartCoroutine(FollowPath(path));
        }
        else
        {
            Debug.Log("No es posible generar un camino. ");
        }
        
    }

    IEnumerator FollowPath(Vector3[] path)
    {
        for (int i = 0; i<path.Length; i++)
        {
            yield return StartCoroutine(GoToPosition(path[i]));
        }
        GetComponent<IdleOrder>().Idle = true;
        this.GetComponent<AbstractNPCBrain>().executed = true;
        this.GetComponent<Entity>()._influenceMapComp.UpdateInfluenceMap(this.GetComponent<Entity>());
    }

    private IEnumerator GoToPosition(Vector3 point)
    {
        bool finished = false;
        Sequence s = DOTween.Sequence();
        s.Append(this.transform.DOMove(point, 1f));
        s.OnComplete(() => { finished = true; });
        while (!finished)
        {
            yield return null;
        }
    }
}
