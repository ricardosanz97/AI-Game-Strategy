using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Action
{
    public Vector3 OnGoingPosition;
    public bool PathReceived = false;
    public override void Act()
    {
        if (!PathReceived)
        {
            return;
        }
    }
}
