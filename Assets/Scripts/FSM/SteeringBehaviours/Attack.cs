using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    public int damage;
    public int range;
    public bool ObjectiveAssigned;
    public AbstractNPCBrain NPCObjectiveAttack;
    public override void Act()
    {
        if (!ObjectiveAssigned)
        {
            return;
        }

        ObjectiveAssigned = false;
        NPCObjectiveAttack.GetComponent<Health>().ReceiveDamage(damage);
        GetComponent<IdleOrder>().Idle = true;
        this.GetComponent<AbstractNPCBrain>().executed = true;
        NPCObjectiveAttack = null;
    }
}
