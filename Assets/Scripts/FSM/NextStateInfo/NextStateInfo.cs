using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

[System.Serializable]
public class NextStateInfo
{
    public State stateCaseTrue;
    public State stateCaseFalse;
    public Order order;
    
    public NextStateInfo (AbstractNPCBrain abstractNpc, STATE stateTrue, STATE stateFalse, Order order)
    {
        this.stateCaseTrue = FSMSystem.FindState(abstractNpc, stateTrue);
        this.stateCaseFalse = FSMSystem.FindState(abstractNpc, stateFalse);
        this.order = order;
    }
	
}
