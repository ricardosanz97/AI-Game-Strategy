using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisionerNPC : Troop
{
    public override void Start()
    {
        base.Start();
        SetStates();
        SetTransitions();
    }

    public override void SetStates()
    {
        SetRemainState();
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
            () =>
            {
                List<CustomPathfinding.Node> nodeList = _pathfindingManager.RequestNodesAtRadius(GetComponent<Attack>().range, transform.position);
                Debug.Log("nodeList tiene " + nodeList.Count + " elementos. ");
                foreach (CustomPathfinding.Node node in nodeList)
                {
                    node.ColorAsPossibleAttackDistance();
                    if (node.cell.troopIn != null && node.cell.troopIn.owner != owner) //the enemy in the cell is an enemy.
                    {
                        possibleAttacks.Add(node);
                        node.GetComponent<CustomPathfinding.Node>().ColorAsPossibleAttack();
                    }
                }
            },
            () =>
            {
                possibleAttacks.Clear();
            }));

        List<Action> behavioursAttackState = new List<Action>()
        {
            GetComponent<Attack>()
        };

        FSMSystem.AddBehaviours(this, behavioursAttackState, states.Find((x) => x.stateName == STATE.Attack));
    }

    private void SetMoveState()
    {
        FSMSystem.AddState(this, new State(STATE.Move, this,
            ()=>
            {
                List<CustomPathfinding.Node> nodeList = _pathfindingManager.RequestNodesAtRadius(GetComponent<Move>().maxMoves, transform.position);
                Debug.Log("nodeList tiene " + nodeList.Count + " elementos. ");
                foreach (CustomPathfinding.Node node in nodeList)
                {
                    possibleMovements.Add(node);
                    node.GetComponent<CustomPathfinding.Node>().ColorAsPossibleMovementDistance();
                }
            },
            ()=>
            {
                possibleMovements.Clear();
                GetInitialDamage();
            }));

        List<Action> behavioursMoveState = new List<Action>()
        {
            GetComponent<Move>()
        };

        FSMSystem.AddBehaviours(this, behavioursMoveState, states.Find((x) => x.stateName == STATE.Move));
    }

    public override void DoAttackAnimation()
    {
        FindObjectOfType<SoundManager>().PlaySingle(FindObjectOfType<SoundManager>().cageSoundAttack);
    }
}
