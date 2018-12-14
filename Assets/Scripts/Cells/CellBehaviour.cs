using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class CellBehaviour : MonoBehaviour
{
    public PlayerType owner;
    public AbstractNPCBrain troopIn;
    private bool PNodeAssigned = false;
    private bool INodeAssigned = false;
    public CustomPathfinding.Node PNode;
    public InfluenceMap.Node INode;

    public List<TurretNPC> explosionBelongsTo = new List<TurretNPC>();

    public void SetOwner(PlayerType playerType)
    {
        this.owner = playerType;
    }

    public void SetTroopIn(AbstractNPCBrain brain)
    {
        this.troopIn = brain;
    }

    public PlayerType GetOwner()
    {
        return this.owner;
    }

    public AbstractNPCBrain GetTroopIn()
    {
        return this.troopIn;
    }

    public void BackToInitialPosition()
    {
        this.transform.DOLocalMoveY(0f, 0.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CustomPathfinding.Node>() && PNode == null)
        {
            Debug.Log("Get reference of the PNode in this cell. ");
            PNode = other.GetComponent<CustomPathfinding.Node>();
            //other.GetComponent<CustomPathfinding.Node>().cell = this;
        }
    }
    /*
    private void OnMouseDown()
    {
        if (this.owner != ENTITY.Player || _turnHandler.currentTurn != ENTITY.Player) //si la casilla no es tipo player o no es nuestro turno -> return
        {
            return;
        }
        _spawnablesManager.SpawnTroop(this.gameObject);
    }
    */

}
