using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNPC : AbstracNPCBrain
{
    public NPC thisNPC = NPC.Tank;

    public override void SetStates()
    {
        SetMoveForwardState();
    }

    public override void SetTransitions()
    {
    }

    public void SetMoveForwardState()
    {
        FSMSystem.AddState(this, new MoveForwardState(thisNPC)); //acabamos de añadir a este NPC el estado MoveForwardState con los comportamientos del tanque.
    }
}
