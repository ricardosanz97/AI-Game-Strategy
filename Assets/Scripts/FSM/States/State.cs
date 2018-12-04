using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State : MonoBehaviour
{    
    public STATE stateName;
    public List<Action> actions;
    
    public State (STATE stateName)
    {
        this.stateName = stateName;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }
}
