using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Action
{
    public int maxMoves;

    [HideInInspector]public CellBehaviour OnGoingCell;
    [HideInInspector]public bool PathReceived = false;
    public override void Act()
    {
        if (!PathReceived)
        {
            return;
        }
    }
}
