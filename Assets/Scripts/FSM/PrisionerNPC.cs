using System.Collections;
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
                foreach (CustomPathfinding.Node node in possibleAttacks)
                {
                    node.cell.gameObject.transform.Find("AttackPlacement").gameObject.SetActive(false);
                }
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
                foreach (CustomPathfinding.Node node in possibleMovements)
                {
                    bool canDisableShader = true;
                    if (this.owner == Owner.Player)
                    {
                        for (int i = 0; i < _levelController.PlayerEntities.Count; i++)
                        {
                            if (_levelController.PlayerEntities[i] == this || _levelController.PlayerEntities[i].GetComponent<Troop>() == null) //si somos nosotros o no es una troop, miramos el siguiente
                            {
                                continue;
          
                            }

                            if (_levelController.PlayerEntities[i].GetComponent<Troop>().possibleMovements.Contains(node))
                            {
                                //Debug.Log("alguien lo contiene, asi que no lo borro. ");
                                canDisableShader = false;
                            }

                        }
                    }

                    else if (this.owner == Owner.AI)
                    {
                        for (int i = 0; i < _levelController.AIEntities.Count; i++)
                        {
                            if (_levelController.AIEntities[i] == this || _levelController.AIEntities[i].GetComponent<Troop>() == null) //si somos nosotros o no es una troop, miramos el siguiente
                            {
                                continue;
                            }

                            if (_levelController.AIEntities[i].GetComponent<Troop>().possibleMovements.Contains(node))
                            {
                                //Debug.Log("Node " + node.GetHashCode() + " no se borra. ");
                                canDisableShader = false;
                            }

                        }
                    }

                    if (canDisableShader)
                    {
                        //Debug.Log("Node " + node.GetHashCode() + " ha sido borrado. ");
                        node.cell.gameObject.transform.Find("MovePlacement").gameObject.SetActive(false);
                    }
                }
                possibleMovements.Clear();
                _pathfindingGrid.UpdateGrid(this);
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
        GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public override void UpgradeNPC()
    {
        base.UpgradeNPC();
        this.UpgradeCost += 2;
        this.GetComponent<Attack>().damage += 2;
        this.GetComponent<Move>().maxMoves++;
    }
}
