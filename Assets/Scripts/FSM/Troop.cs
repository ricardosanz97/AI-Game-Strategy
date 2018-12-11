using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Zenject;

[RequireComponent(typeof(MoveOrder))]
[RequireComponent(typeof(AttackOrder))]
[RequireComponent(typeof(IdleOrder))]

public class Troop : AbstractNPCBrain
{
    [Inject]
    public PathfindingManager _pathfindingManager;

    public override void Awake()
    {
        base.Awake();
        _pathfindingManager = FindObjectOfType<PathfindingManager>();
    }

    public override void Start()
    {
        initialState = new State(STATE.Idle, this, ()=> { }, ()=> { });
        FSMSystem.AddState(this, initialState);
        SetStates();
        SetTransitions();
        currentState = initialState;
        currentTransitions = transitions.FindAll((x) => x.currentState == currentState);

        base.Start();
    }


    public void OnMouseDown()
    {
        Debug.Log("click in enemy");
        if (_levelController.GetAnyPopupEnabled())
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

    public override void SetTransitions()
    {
    }

    public override void SetStates()
    {
    }
}
