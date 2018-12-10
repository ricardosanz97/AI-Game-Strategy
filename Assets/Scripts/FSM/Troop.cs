using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveOrder))]
[RequireComponent(typeof(AttackOrder))]
[RequireComponent(typeof(IdleOrder))]
public class Troop : AbstractNPCBrain
{
public override void Start()
    {
        initialState = new State(STATE.Idle, this);
        FSMSystem.AddState(this, initialState);
        SetStates();
        SetTransitions();
        currentState = initialState;
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);

        base.Start();
    }

    public override void SetStates()
    {
        FSMSystem.AddState(this, new State(STATE.Remain, this));
        FSMSystem.AddState(this, new State(STATE.Move, this));
        FSMSystem.AddState(this, new State(STATE.Attack, this));
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> nextStatesInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.Remain, GetComponent<AttackOrder>()),
            new NextStateInfo(this, STATE.Move, STATE.Remain, GetComponent<MoveOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Idle, nextStatesInfo2);

        List<NextStateInfo> nextStateInfo3 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.Remain, GetComponent<IdleOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Move, nextStateInfo3);

        List<NextStateInfo> nextStateInfo4 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Idle, STATE.Remain, GetComponent<IdleOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Attack, nextStateInfo4);
    }

    public void OnMouseDown()
    {
        Debug.Log("click in enemy");
        if (popupOptionsEnabled)
        {
            return;
        }
        popupOptionsEnabled = true;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleOptionsPopup"));
        go.GetComponent<SimpleOptionsPopupController>().SetPopup(
        this.transform.localPosition,
        this.npc.ToString(),
        "Mover",
        "Atacar",
        () => {
            GetComponent<MoveOrder>().Move = true;
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;

        },
        () => {
            GetComponent<AttackOrder>().Attack = true;
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;

        },
        () =>
        {
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;
        });

    }
}
