using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.Events;

[System.Serializable]
public class State
{
    public STATE stateName;
    public AbstractNPCBrain brain;
    public List<Action> actions;
    private StateInfo _stateInfo;

    public UnityAction OnEnter;
    public UnityAction OnExit;

    public State(STATE stateName, AbstractNPCBrain brain, UnityAction onEnter = null, UnityAction onExit = null)
    {
        _stateInfo = GameObject.FindObjectOfType<StateInfo>().GetComponent<StateInfo>();
        this.stateName = stateName;
        this.brain = brain;
        this.OnEnter = onEnter;
        this.OnExit = onExit;
    }

    /*
    public void OnEnter(bool stateByDefault = false, bool npcByDefault = false)
    {
        _stateInfo.ExecuteOnEnter(stateByDefault ? STATE.None : stateName, npcByDefault ? TROOP.None : brain.npc);
    }

    public void OnExit(bool stateByDefault = false, bool npcByDefault = false) 
    {
        _stateInfo.ExecuteOnExit(stateByDefault ? STATE.None : stateName, npcByDefault ? TROOP.None : brain.npc);
    }
    */

}
