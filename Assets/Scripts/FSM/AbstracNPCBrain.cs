using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public enum NPC
{
    Minion,
    Archer,
    Tank,
    None
}

public abstract class AbstracNPCBrain : MonoBehaviour
{    
    public abstract void SetTransitions();
    public abstract void SetStates();

    public List<Transition> transitions;
    public List<State> states;
    public State currentState;
    public State initialState;
    public List<Transition> currentTransitions;

    public virtual void Start()
    {
        SetInitialState();
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

    public virtual void CheckConditions()
    {
        foreach (Transition trans in currentTransitions)
        {
            foreach (NextStateInfo nsi in trans.nextStateInfo)
            {
                bool result = nsi.changeCondition.Check();
                
                if (result)
                {
                    if (nsi.stateCaseTrue.stateName == STATE.None)
                    {
                        continue;
                    }
                    currentState.OnExit();
                    currentState = nsi.stateCaseTrue;
                    currentState.OnEnter();
                }
                else
                {
                    if (nsi.stateCaseFalse.stateName == STATE.None)
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

    public void SetInitialState()
    {
        this.currentState = this.initialState;
        this.currentState.OnEnter();
        currentTransitions = this.transitions.FindAll((x) => x.currentState == this.currentState);
    }

    
}
