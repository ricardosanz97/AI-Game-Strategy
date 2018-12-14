using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherNPC : Troop
{
    public override void SetStates()
    {
        base.SetStates();
        FSMSystem.AddState(this, new State(STATE.Remain, this));
        SetAttackState();    
        SetMoveState();  
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
    private void SetAttackState()
    {
        FSMSystem.AddState(this, new State(STATE.Attack, this,
            () => {//on enter attack state
                CustomPathfinding.Node[] nodeList = _pathfindingManager.RequestNodesAtRadius(GetComponent<Attack>().range, transform.position);
                Debug.Log("nodeList tiene " + nodeList.Length + " elementos. ");
                foreach (CustomPathfinding.Node node in nodeList)
                {
                    node.ColorAsPossibleAttackDistance();
                    if (node.cell.troopIn != null && node.cell.troopIn.owner != owner) //the enemy in the cell is an enemy.
                    {
                        possibleAttacks.Add(node);
                        node.GetComponent<CustomPathfinding.Node>().ColorAsPossibleAttack();
                    }
                }
                foreach (CustomPathfinding.Node node in nodeList)
                {
                      
                }
                tryingTo = TryingTo.Attack;
            },
            () => {
                Debug.Log("ONEXIT!");
            })
        );

        List<Action> behavioursAttackState = new List<Action>()
        {
            GetComponent<Attack>()
        };

        FSMSystem.AddBehaviours(this, behavioursAttackState, states.Find((x) => x.stateName == STATE.Attack));
    }

    public void SetMoveState()
    {
        FSMSystem.AddState(this, new State(STATE.Move, this,
            () =>//on enter move state
            {
                CustomPathfinding.Node[] nodeList = _pathfindingManager.RequestNodesAtRadius(GetComponent<Move>().maxMoves, transform.position);
                Debug.Log("nodeList tiene " + nodeList.Length + " elementos. ");
                foreach (CustomPathfinding.Node node in nodeList)
                {
                    possibleMovements.Add(node);
                    node.GetComponent<CustomPathfinding.Node>().ColorAsPossibleMovementDistance();
                }
                tryingTo = TryingTo.Move;
            },
            () =>
            {
            })
        );

        List<Action> behavioursMoveState = new List<Action>()
        {
            GetComponent<Move>()
        };

        FSMSystem.AddBehaviours(this, behavioursMoveState, states.Find((x) => x.stateName == STATE.Move));
    }

}
