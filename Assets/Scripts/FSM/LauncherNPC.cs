using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherNPC : Troop
{
    public override void SetStates()
    {
        base.SetStates();
        FSMSystem.AddState(this, new State(STATE.Remain, this));
        FSMSystem.AddState(this, new State(STATE.Move, this, 
            ()=>
            {
                CustomPathfinding.Node[] nodeList = _pathfindingManager.RequestWalkableNodesAtRadius(GetComponent<Move>().maxMoves, transform.position);
                Debug.Log("nodeList tiene " + nodeList.Length + " elementos. ");
                foreach (CustomPathfinding.Node node in nodeList)
                {
                    node.GetComponent<MeshRenderer>().material.color = Color.black;
                }
            },
            ()=> 
            {
            }));
        FSMSystem.AddState(this, new State(STATE.Attack, this, 
            ()=> {
                CustomPathfinding.Node[] nodeList = _pathfindingManager.RequestWalkableNodesAtRadius(GetComponent<Attack>().range, transform.position);
                Debug.Log("nodeList tiene " + nodeList.Length + " elementos. ");
                foreach (CustomPathfinding.Node node in nodeList)
                {
                    node.GetComponent<MeshRenderer>().material.color = Color.black;
                }
            }, 
            ()=> {
                Debug.Log("ONEXIT!");
            })
        );
    }

    public override void SetTransitions()
    {
        base.SetTransitions();
        List<NextStateInfo> nextStatesInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.Remain, GetComponent<AttackOrder>()),
            new NextStateInfo(this, STATE.Move, STATE.Remain, GetComponent<MoveOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Idle, nextStatesInfo2);

        List<NextStateInfo> nextStateInfo3 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.Remain, GetComponent<IdleOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Move, nextStateInfo3);

        List<NextStateInfo> nextStateInfo4 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.Remain, GetComponent<IdleOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Attack, nextStateInfo4);
    }
}
