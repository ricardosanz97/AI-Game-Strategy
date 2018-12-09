using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOrder : Order
{
    [HideInInspector] public bool Move = false;
    public override bool Check()
    {
        if (Move)
        {
            Move = false;
            Debug.Log("MOVE ORDER!");
            return true;
        }
        else
        {
            return false;
        }
    }
}
