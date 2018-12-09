using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOrder : Order {

    [HideInInspector] public bool Attack = false;
    public override bool Check()
    {
        if (Attack)
        {
            Attack = false;
            Debug.Log("ATTACK ORDER!");
            return true;
        }
        else
        {
            return false;
        }
    }
}
