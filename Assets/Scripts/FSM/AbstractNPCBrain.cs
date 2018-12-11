﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public abstract class AbstractNPCBrain : Entity
{
    public TROOP npc = TROOP.None;
    public float healthPoints = 100f;
    public int currentLevel = 1;


    public abstract void SetTransitions();
    public abstract void SetStates();

    [HideInInspector]public List<Transition> transitions;
    [HideInInspector]public List<State> states;
    public State currentState;
    [HideInInspector]public State initialState;
    public List<Transition> currentTransitions;

    public bool popupOptionsEnabled = false;

    public virtual void Start()
    {
        if (npc == TROOP.None)
        {
            Debug.LogError("NPC type unassigned. ");
        }
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
                    currentState.OnExit();
                    currentState = nsi.stateCaseTrue;
                    currentState.OnEnter();
                }
                else
                {
                    if (nsi.stateCaseFalse.stateName == STATE.Remain)
                    {
                        continue;
                    }
                    currentState.OnExit();
                    currentState = nsi.stateCaseFalse;
                    currentState.OnEnter();
                }
                currentTransitions = transitions.FindAll(x => x.currentState == currentState);
                return;
            }
        }
    }

    private void Update()
    {
        CheckOrder();
        ActBehaviours();
    }


}