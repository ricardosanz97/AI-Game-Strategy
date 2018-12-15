using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using CustomPathfinding;
using Zenject;

[RequireComponent(typeof(MoveOrder))]
[RequireComponent(typeof(AttackOrder))]
[RequireComponent(typeof(IdleOrder))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Move))]
public class Troop : AbstractNPCBrain
{
    public List<CustomPathfinding.Node> possibleMovements;
    public List<CustomPathfinding.Node> possibleAttacks;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        possibleMovements = new List<CustomPathfinding.Node>();

        initialState = new State(STATE.Idle, this,
            () => {
                if (possibleMovements.Count > 0)
                {
                    foreach (CustomPathfinding.Node node in possibleMovements)
                    {

                    }
                }
                GetInitialDamage();
            },
            () => {
            }
        );

        FSMSystem.AddState(this,initialState);
        currentState = states.Find((x) => x.stateName == STATE.Idle);

        base.Start();

        //xecuted = true;
    }


    public void OnMouseDown()
    {
        Debug.Log("click in enemy");
        if (_levelController.GetAnyPopupEnabled() || this.owner == Owner.AI || this.GetComponent<AbstractNPCBrain>().executed || this.GetComponent<AbstractNPCBrain>().currentState.stateName != STATE.Idle) //si hay algun popup abierto o el pertenece a la IA no te abras.
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

    public bool ListPossibleMovementsContains(CustomPathfinding.Node node)
    {
        for (int i = 0; i<possibleMovements.Count; i++)
        {
            if (possibleMovements[i].gameObject == node.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    public bool ListPossibleAttacksContains(CustomPathfinding.Node node)
    {
        for (int i = 0; i<possibleAttacks.Count; i++)
        {
            if (possibleAttacks[i].gameObject == node.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    public override void SetTransitions()
    {
    }

    public override void SetStates()
    {
    }
}
