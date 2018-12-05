using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State
{
    public STATE stateName;
    public AbstracNPCBrain brain;
    public List<Action> actions;
    public State(STATE stateName, AbstracNPCBrain brain)
    {
        this.stateName = stateName;
        this.brain = brain;
    }

    public void OnEnter(bool stateByDefault = false, bool npcByDefault = false)
    {
        StateInfo.ExecuteOnEnter(stateByDefault ? STATE.None : stateName, npcByDefault ? TROOP.None : brain.npc);
    }

    public void OnExit(bool stateByDefault = false, bool npcByDefault = false) 
    {
        StateInfo.ExecuteOnExit(stateByDefault ? STATE.None : stateName, npcByDefault ? TROOP.None : brain.npc);
    }

}
