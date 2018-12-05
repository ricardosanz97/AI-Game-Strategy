using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNPC : AbstracNPCBrain
{

    public override void Start()
    {
        initialState = new State(STATE.Idle, this);
    }

    public override void SetStates()
    {
    }

    public override void SetTransitions()
    {
    }
}
