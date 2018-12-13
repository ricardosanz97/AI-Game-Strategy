using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNPC : AbstractNPCBrain
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
    }

    public void OnMouseDown()
    {
        Debug.Log("click in enemy");
        if (_levelController.GetAnyPopupEnabled() || this.owner == Owner.AI || this.GetComponent<AbstractNPCBrain>().executed)
        {
            return;
        }
        popupOptionsEnabled = true;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleOptionsPopup"));
        go.GetComponent<SimpleOptionsPopupController>().SetPopup(
        this.transform.localPosition,
        this.npc.ToString(),
        "RIGHT",
        "LEFT",
        () => {
            //rotate 90
            RotateRight();
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;

        },
        () => {
            //rotate -90
            RotateLeft();
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;
        },
        () =>
        {
            go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
            popupOptionsEnabled = false;
        });
    }

    private void RotateRight()
    {
        this.transform.Rotate(Vector3.up * 90f);
    }
    
    private void RotateLeft()
    {
        this.transform.Rotate(Vector3.up * -90f);
    }
}
