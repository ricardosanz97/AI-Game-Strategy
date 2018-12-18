﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisionerNPC : Troop
{
    public override void Start()
    {
        base.Start();
        //ya hemos iniciado a currentState
        SetStates();
        SetTransitions();

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
                _pathfindingGrid.UpdateGrid(this);
                GetCellsWithEnemiesInRange();
            },
            () =>
            {
                DisableShaderAttackCells();
                possibleAttacks.Clear();
                _pathfindingGrid.UpdateGrid(this);
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
                _pathfindingGrid.UpdateGrid(this);
                GetCellsPossibleMovements();
            },
            ()=>
            {
                DisableShaderMoveCells();
                possibleMovements.Clear();
                _pathfindingGrid.UpdateGrid(this);
            }));

        List<Action> behavioursMoveState = new List<Action>()
        {
            GetComponent<Move>()
        };

        FSMSystem.AddBehaviours(this, behavioursMoveState, states.Find((x) => x.stateName == STATE.Move));
    }

    [ContextMenu("Attack Animation")]
    public override void DoAttackAnimation()
    {
        FindObjectOfType<SoundManager>().PlaySingle(FindObjectOfType<SoundManager>().cageSoundAttack);
        GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public override void UpgradeNPC()
    {
        bool bloodEnough = this.owner == Entity.Owner.Player ? this.UpgradeCost < _bloodController.PlayerBlood : this.UpgradeCost < _bloodController.AIBlood;
        if (currentLevel > MaxUpgradeLevel)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup(this.entityType.ToString(), "MAX LEVEL\nREACHED");
            return;
        }
        if (bloodEnough)
        {
            if (currentLevel > MaxUpgradeLevel)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup(this.entityType.ToString(), "MAX LEVEL\nREACHED");
                return;
            }
            base.UpgradeNPC();
            this.UpgradeCost += 3;
            this.GetComponent<Attack>().damage+=2;
            this.GetComponent<Move>().maxMoves++;
        }
        else
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "NOT ENOUGH\nBLOOD");
        }
    }
}
