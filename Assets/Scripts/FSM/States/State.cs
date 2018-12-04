using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State : MonoBehaviour
{
    public NPC owner;
    public List<Action> actions;
    public State(NPC owner)
    {
        this.owner = owner;
    }

    public abstract void OnEnter();
    public abstract void OnExit();

}
