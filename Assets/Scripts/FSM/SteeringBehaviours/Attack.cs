using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    public int damage;
    public int range;
    public bool ObjectiveAssigned;
    public Entity targetEntity;
    public float WaitTimeUntilAttack = 1.0f;
    
    public override void Act()
    {
        if (!ObjectiveAssigned)
        {
            return;
        }

        ObjectiveAssigned = false;
        targetEntity.GetComponent<Health>().ReceiveDamage(damage);
        GetComponent<IdleOrder>().Idle = true;
        this.GetComponent<AbstractNPCBrain>().executed = true;
        targetEntity = null;
    }
}
