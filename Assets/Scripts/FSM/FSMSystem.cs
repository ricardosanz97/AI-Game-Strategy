using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum STATE
{
    Default,
    Attack,
    Patrol,
    Idle,
    Alert,
    Recolocate,
    Hidden,
    None
}

public enum BEHAVIOUR
{
    Patrol,
    Hit,
    Shoot,
    Rotate,
}

public static class FSMSystem{

	public static void AddTransition(AbstracNPCBrain abstracNpc, STATE currentStateName, List<NextStateInfo> nextStateInfos)
    {
        State _currentState = abstracNpc.states.Find((x) => x.stateName == currentStateName);
        List<NextStateInfo> _nextStateInfos = nextStateInfos;

        Transition _transition = new Transition(_currentState, _nextStateInfos);
        foreach (Transition trans in abstracNpc.transitions)
        {
            if (trans == _transition) //TODO: falta delimitarlo más.
            {
                Debug.Log("Can't add it because there already is a transition like this");
            }
        }
        abstracNpc.transitions.Add(_transition);
    }

    public static void DeleteTransition(AbstracNPCBrain abstracNpc, State currentState, List<NextStateInfo> nextStatesInfos)
    {
        Transition _transition = new Transition(currentState, nextStatesInfos);
        foreach (Transition trans in abstracNpc.transitions)
        {
            if (trans == _transition)
            {
                abstracNpc.transitions.Remove(trans);
            }
        }
    }

    public static void AddState(AbstracNPCBrain abstracNpc, State newState)
    {
        abstracNpc.states.Add(newState);
    }

    public static void AddBehaviours(AbstracNPCBrain abstracNpc, List<Action> _behaviours, State _state)
    {
        foreach (State state in abstracNpc.states)
        {
            if (state == _state)
            {
                state.actions = _behaviours;
            }
        }
    }

    public static State GetNextState(AbstracNPCBrain abstracNpc, bool _bool, State currentState, Condition condition)
    {
        foreach (Transition trans in abstracNpc.transitions)
        {
            if (trans.currentState == currentState)
            {
                foreach (NextStateInfo nsi in trans.nextStateInfo)
                {
                    if (condition == nsi.changeCondition && _bool)
                    {
                        return nsi.stateCaseTrue;
                    }
                    else if (condition == nsi.changeCondition && !_bool)
                    {
                        return nsi.stateCaseFalse;
                    }
                }
            }
        }
        return null;
    }

    public static State FindState(AbstracNPCBrain abstracNpc, STATE stateName)
    {
        State returnState = abstracNpc.states.Find((x) => x.stateName == stateName);
        return returnState;
    }
}
