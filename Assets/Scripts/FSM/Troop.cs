using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Zenject;

[RequireComponent(typeof(MoveOrder))]
[RequireComponent(typeof(AttackOrder))]
[RequireComponent(typeof(IdleOrder))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Move))]
public class Troop : AbstractNPCBrain
{
    [Inject]
    [HideInInspector]public PathfindingManager _pathfindingManager;
    public List<CustomPathfinding.Node> possibleMovements;
    public List<CustomPathfinding.Node> possibleAttacks;

    public override void Awake()
    {
        base.Awake();
        _pathfindingManager = FindObjectOfType<PathfindingManager>();
    }

    public override void Start()
    {
        possibleMovements = new List<CustomPathfinding.Node>();

        initialState = new State(STATE.Idle, this, 
            ()=> {
                if (possibleMovements.Count > 0)
                {
                    foreach (CustomPathfinding.Node node in possibleMovements)
                    {
                        node.GetComponent<CustomPathfinding.Node>().ResetColor();
                    }
                }
            }, 
            ()=> {

            }
        );
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
        if (_levelController.GetAnyPopupEnabled() || this.owner == Owner.AI || this.GetComponent<AbstractNPCBrain>().executed) //si hay algun popup abierto o el pertenece a la IA no te abras.
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
}
