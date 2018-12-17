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
                //GetTurretDamage();
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

    public void GetCellsWithEnemyConstructionsInRange()
    {
        List<CustomPathfinding.Node> nodeList = _pathfindingManager.RequestNodesAtRadiusWithObstacles(GetComponent<Attack>().range, transform.position);
        foreach (CustomPathfinding.Node node in nodeList)
        {
            if (node.cell.entityIn != null && node.cell.entityIn.GetComponent<Troop>() == null && node.cell.entityIn.owner != owner) //the enemy in the cell is an enemy.
            {
                node.cell.gameObject.transform.Find("AttackPlacement").gameObject.SetActive(true);
                possibleAttacks.Add(node);
            }
        }
        Debug.Log("possibleAttacks.Count = " + possibleAttacks.Count);
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

    public void DisableShaderMoveCells()
    {
        foreach (CustomPathfinding.Node node in possibleMovements)
        {
            bool canDisableShader = true;
            if (this.owner == Owner.Player)
            {
                for (int i = 0; i < _levelController.PlayerEntities.Count; i++)
                {
                    if (_levelController.PlayerEntities[i] == this || _levelController.PlayerEntities[i].GetComponent<Troop>() == null) //si somos nosotros o no es una troop, miramos el siguiente
                    {
                        continue;
                    }

                    if (_levelController.PlayerEntities[i].GetComponent<Troop>().possibleMovements.Contains(node))
                    {
                        //Debug.Log("alguien lo contiene, asi que no lo borro. ");
                        canDisableShader = false;
                    }
                }
            }

            else if (this.owner == Owner.AI)
            {
                for (int i = 0; i < _levelController.AIEntities.Count; i++)
                {
                    if (_levelController.AIEntities[i] == this || _levelController.AIEntities[i].GetComponent<Troop>() == null) //si somos nosotros o no es una troop, miramos el siguiente
                    {
                        continue;
                    }

                    if (_levelController.AIEntities[i].GetComponent<Troop>().possibleMovements.Contains(node))
                    {
                        //Debug.Log("Node " + node.GetHashCode() + " no se borra. ");
                        canDisableShader = false;
                    }
                }
            }

            //if (canDisableShader)
            //{
                //Debug.Log("Node " + node.GetHashCode() + " ha sido borrado. ");
                node.cell.gameObject.transform.Find("MovePlacement").gameObject.SetActive(false);
            //}
        }
    }

    public void DisableShaderAttackCells()
    {
        foreach (CustomPathfinding.Node node in possibleAttacks)
        {
            node.cell.gameObject.transform.Find("AttackPlacement").gameObject.SetActive(false);
        }
    }

}
