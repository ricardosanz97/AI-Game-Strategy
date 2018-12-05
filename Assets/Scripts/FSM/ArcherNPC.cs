using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherNPC : AbstracNPCBrain
{
    public override void Start()
    {
        initialState = new State(STATE.Idle, this);
        SetInitialState();
    }

    public override void SetStates()
    {

    }

    public override void SetTransitions()
    {
            
    }
}
