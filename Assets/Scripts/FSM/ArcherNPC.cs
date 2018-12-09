using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherNPC : AbstracNPCBrain
{
    public override void Start()
    {
        initialState = new State(STATE.Idle, this);
        SetStates();
        SetTransitions();

        currentState = initialState;
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);

        base.Start();
    }

    public override void SetStates()
    {
        FSMSystem.AddState(this, new State(STATE.Remain, this));
        FSMSystem.AddState(this, new State(STATE.Idle, this));
        FSMSystem.AddState(this, new State(STATE.Move, this));
        FSMSystem.AddState(this, new State(STATE.Attack, this));
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> nextStatesInfo = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Move, STATE.Remain, GetComponent<MoveOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Idle, nextStatesInfo);

        List<NextStateInfo> nextStatesInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.Remain, GetComponent<AttackOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Idle, nextStatesInfo2);
    }
}
