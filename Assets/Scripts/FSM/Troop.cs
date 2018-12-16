using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using CustomPathfinding;
using ModestTree;
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
    public PathfindingGrid _pathfindingGrid;

    public override void Awake()
    {
        base.Awake();
        _pathfindingGrid = FindObjectOfType<PathfindingGrid>();
    }

    public override void Start()
    {
        possibleMovements = new List<CustomPathfinding.Node>();

        initialState = new State(STATE.Idle, this,
            () => {
                GetTurretDamage();
                _pathfindingGrid.UpdateGrid(this);
            },
            () => {
                _pathfindingGrid.UpdateGrid(this);
            }
        );

        FSMSystem.AddState(this,initialState);
        currentState = states.Find((x) => x.stateName == STATE.Idle);

        base.Start();

        executed = true;
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
        this.entityType.ToString(),
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
            this.UpgradeNPC();
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

    public void GetCellsWithEnemiesInRange()
    {
        List<CustomPathfinding.Node> nodeList = _pathfindingManager.RequestNodesAtRadiusWithObstacles(GetComponent<Attack>().range, transform.position);
        foreach (CustomPathfinding.Node node in nodeList)
        {
            if (node.cell.entityIn != null && node.cell.entityIn.owner != owner) //the enemy in the cell is an enemy.
            {
                node.cell.gameObject.transform.Find("AttackPlacement").gameObject.SetActive(true);
                possibleAttacks.Add(node);
            }
        }
        Debug.Log("possibleAttacks.Count = " + possibleAttacks.Count);
    }

    public void GetCellsPossibleMovements()
    {
        List<CustomPathfinding.Node> nodeList = _pathfindingManager.RequestNodesAtRadius(GetComponent<Move>().maxMoves, transform.position);
        Debug.Log("nodeList tiene " + nodeList.Count + " elementos. ");
        foreach (CustomPathfinding.Node node in nodeList)
        {
            Debug.Log("generando lista de posibles movimientos. ");
            if (this.owner == Owner.Player)
            {
                if (this.cell.PNode.GridX < node.GridX)
                {
                    node.cell.gameObject.transform.Find("MovePlacement").gameObject.SetActive(true);
                    possibleMovements.Add(node);
                }
            }
            else
            {
                if (this.cell.PNode.GridX > node.GridX)
                {
                    node.cell.gameObject.transform.Find("MovePlacement").gameObject.SetActive(true);
                    possibleMovements.Add(node);
                }
            }
        }
    }

}
