using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class CellBehaviour : MonoBehaviour
{
    public ENTITY owner;
    public AbstracNPCBrain troopIn;

    public void SetOwner(ENTITY entity)
    {
        this.owner = entity;
    }

    public void SetTroopIn(AbstracNPCBrain brain)
    {
        this.troopIn = brain;
    }

    public ENTITY GetOwner()
    {
        return this.owner;
    }

    public AbstracNPCBrain GetTroopIn()
    {
        return this.troopIn;
    }

    public void BackToInitialPosition()
    {
        this.transform.DOLocalMoveY(0f, 0.3f);
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
