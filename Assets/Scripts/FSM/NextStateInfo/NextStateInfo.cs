using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

[System.Serializable]
public class NextStateInfo
{
    [Inject] private FSMSystem _fsmSystem;
    public State stateCaseTrue;
    public State stateCaseFalse;
    public Condition changeCondition;
    
    public NextStateInfo (AbstracNPCBrain abstractNpc, STATE stateTrue, STATE stateFalse, Condition changeCondition)
    {
        this.stateCaseTrue = _fsmSystem.FindState(abstractNpc, stateTrue);
        this.stateCaseFalse = _fsmSystem.FindState(abstractNpc, stateFalse);
        this.changeCondition = changeCondition;
    }
	
}
