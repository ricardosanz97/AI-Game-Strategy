﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionNPC : AbstractNPCBrain
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
        FSMSystem.AddState(this, new State(STATE.Attack, this));
    }

    public override void SetTransitions()
    {
        List<NextStateInfo> nextStatesInfo2 = new List<NextStateInfo>()
        {
            new NextStateInfo(this, STATE.Attack, STATE.Remain, GetComponent<AttackOrder>())
        };
        FSMSystem.AddTransition(this, STATE.Idle, nextStatesInfo2);
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
        "Rotate 90",
        "Rotate -90",
        () => {
            //rotate 90
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;

        },
        () => {
            //rotate -90
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
