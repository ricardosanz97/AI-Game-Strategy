using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNPC : AbstracNPCBrain
{
    public override void SetStates()
    {
        states.Add(new State(STATE.Idle));
        states.Add(new State(STATE.Attack));
        states.Add(new State(STATE.Alert));
    }

    public override void SetTransitions()
    {
    }
}
