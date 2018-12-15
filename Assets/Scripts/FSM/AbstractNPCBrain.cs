using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Reflection;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public abstract class AbstractNPCBrain : Entity
{
    public int currentLevel = 1;
    [SerializeField]public Slider sliderHealth;


    public abstract void SetTransitions();
    public abstract void SetStates();

    [HideInInspector]public List<Transition> transitions;
    [HideInInspector]public List<State> states;
    [HideInInspector]public State initialState;
    public List<Transition> currentTransitions;
    public State currentState;
    [HideInInspector]public bool popupOptionsEnabled = false;
    [HideInInspector]public Pathfinding.PathfindingManager _pathfindingManager;


    public bool executed = false;

    public override void Awake()
    {
        base.Awake();
        sliderHealth = this.GetComponentInChildren<Slider>();
        _pathfindingManager = FindObjectOfType<Pathfinding.PathfindingManager>();
    }

    public virtual void Start()
    {
        if (entityType == ENTITY.None)
        {
            Debug.LogError("NPC type unassigned. ");
        }
        sliderHealth.maxValue = GetComponent<Health>().health;
        sliderHealth.value = sliderHealth.maxValue;
    }

    public void SetRemainState()
    {
        FSMSystem.AddState(this, new State(STATE.Remain, this));
    }

    public virtual void ActBehaviours()
    {
        if (currentState.actions != null)
        {
            foreach (Action action in currentState.actions)
            {
                action.Act();
            }
        }
    }

    public virtual void CheckOrder()
    {
        foreach (Transition trans in currentTransitions)
        {
            foreach (NextStateInfo nsi in trans.nextStateInfo)
            {
                bool result = nsi.order.Check();
                //Debug.Log(nsi.order.ToString());
                
                if (result)
                {
                    if (nsi.stateCaseTrue.stateName == STATE.Remain)
                    {
                        continue;
                    }
                    currentState.OnExit?.Invoke();
                    currentState = nsi.stateCaseTrue;
                    currentState.OnEnter?.Invoke();
                }
                else
                {
                    if (nsi.stateCaseFalse.stateName == STATE.Remain)
                    {
                        continue;
                    }
                    currentState.OnExit?.Invoke();
                    currentState = nsi.stateCaseFalse;
                    currentState.OnEnter?.Invoke();
                }
                currentTransitions = transitions.FindAll(x => x.currentState == currentState);
                return;
            }
        }
    }

    public void GetInitialDamage()
    {
        CellBehaviour cell = this.cell;
        if (cell.explosionBelongsTo.Count > 0)
        {
            foreach (TurretNPC t in cell.explosionBelongsTo)
            {
                this.GetComponent<Health>().ReceiveDamage(t.GetComponent<AreaAttack>().damage);
            }
        }
        Debug.Log("get initial damage. ");

    }

    public virtual void DoAttackAnimation()
    {

    }

    private void Update()
    {
        CheckOrder();
        ActBehaviours();
    }


}
