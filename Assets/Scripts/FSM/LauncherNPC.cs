using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherNPC : Troop
{
    public override void Start()
    {
        base.Start();
        //ya hemos iniciado a currentState
        SetStates();
        SetTransitions();

        GetComponentInChildren<ParticleSystem>().Stop();

        currentTransitions = transitions.FindAll((x) => x.currentState.stateName == currentState.stateName);
        Debug.Log("current transitions es " + currentTransitions.Count);
        currentState.OnEnter();
    }

    public override void SetStates()
    {
        SetRemainState();
        SetAttackState();    
        SetMoveState();  
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> nextStatesInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.Remain, GetComponent<AttackOrder>()),
            new NextStateInfo(this, STATE.Move, STATE.Remain, GetComponent<MoveOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Idle, nextStatesInfo2);

        List<NextStateInfo> nextStateInfo3 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.Remain, GetComponent<IdleOrder>()),
            new NextStateInfo(this, STATE.Attack, STATE.Remain, GetComponent<AttackOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Move, nextStateInfo3);

        List<NextStateInfo> nextStateInfo4 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.Remain, GetComponent<IdleOrder>()),
            new NextStateInfo(this, STATE.Move, STATE.Remain, GetComponent<MoveOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Attack, nextStateInfo4);
    }
    private void SetAttackState()
    {
        FSMSystem.AddState(this, new State(STATE.Attack, this,
            () => {//on enter attack state
                GetCellsWithEnemiesInRange();
                _pathfindingManager.GetComponent<CustomPathfinding.PathfindingGrid>().UpdateGrid(null);
            },
            () => {
                foreach (CustomPathfinding.Node node in possibleAttacks)
                {
                    node.cell.gameObject.transform.Find("AttackPlacement").gameObject.SetActive(false);
                }
                possibleAttacks.Clear();
                _pathfindingManager.GetComponent<CustomPathfinding.PathfindingGrid>().UpdateGrid(null);
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
                GetCellsPossibleMovements();
                _pathfindingManager.GetComponent<CustomPathfinding.PathfindingGrid>().UpdateGrid(null);
            },
            () =>
            {
                Debug.Log("hacemos el onexit");
                foreach (CustomPathfinding.Node node in possibleMovements)
                {
                    node.cell.gameObject.transform.Find("MovePlacement").gameObject.SetActive(false);
                }
                possibleMovements.Clear();
                _pathfindingManager.GetComponent<CustomPathfinding.PathfindingGrid>().UpdateGrid(null);
            })
        );

        List<Action> behavioursMoveState = new List<Action>()
        {
            GetComponent<Move>()
        };

        FSMSystem.AddBehaviours(this, behavioursMoveState, states.Find((x) => x.stateName == STATE.Move));
    }

    public override void DoAttackAnimation()
    {
        GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public override void UpgradeNPC()
    {
        Debug.Log("UpgradeNPC");
        base.UpgradeNPC();
        this.UpgradeCost += 2;
        this.GetComponent<Attack>().damage++;
        this.GetComponent<Attack>().range++;
        this.GetComponent<Move>().maxMoves++;
    }
}
