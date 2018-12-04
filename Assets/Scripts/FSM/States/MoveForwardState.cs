using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardState : State
{
    public MoveForwardState(NPC owner) : base(owner)
    {
        switch (owner) {
            case NPC.Minion:
                //actions for minion
                break;
            case NPC.Archer:
                //actions for archer
                break;
            case NPC.Tank:
                //actions.Add(this.GetComponent<InspectNextCell>();
                //actions.Add(this.GetComponent<MoveForward>();
                break;
        }
    }

    public override void OnEnter()
    {
        switch (owner)
        {
            case NPC.Minion:
                //OnEnter for the minion in this state
                break;
            case NPC.Archer:
                //OnEnter for the archer in this state
                break;
            case NPC.Tank:
                //OnEnter for the tank in this state
                break;
        }
    }

    public override void OnExit()
    {
        switch (owner)
        {
            case NPC.Minion:
                //OnExit for the minion in this state
                break;
            case NPC.Archer:
                //OnExit for the archer in this state
                break;
            case NPC.Tank:
                //OnExit for the tank in this state
                break;
        }
    }
}
