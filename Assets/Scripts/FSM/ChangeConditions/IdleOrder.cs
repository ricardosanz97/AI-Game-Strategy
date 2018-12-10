using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleOrder : Order
{
    [HideInInspector] public bool Idle = false;
    public override bool Check()
    {
        if (Idle)
        {
            Idle = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
