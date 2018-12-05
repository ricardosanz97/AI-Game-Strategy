using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class CellBehaviour : MonoBehaviour
{
    [Inject]
    private SpawnablesManager _spawnablesManager;
    [Inject]
    private TurnHandler _turnHandler;
    private bool canClick = false;
    public ENTITY owner;
    public AbstracNPCBrain troopIn;

    private void Awake()
    {
        _spawnablesManager = GameObject.FindObjectOfType<SpawnablesManager>().GetComponent<SpawnablesManager>();
        _turnHandler = GameObject.FindObjectOfType<TurnHandler>().GetComponent<TurnHandler>();
    }

    private void OnMouseOver()
    {
        if (_spawnablesManager.GetCurrentTroop() == TROOP.None || _turnHandler.currentTurn != ENTITY.Player || owner != ENTITY.Player) //si no hay tropa seleccionada o no es el turno del player o la casilla no es del player...
        {
            return;
        }
        this.transform.DOLocalMoveY(0.5f, 0f);
    }

    private void OnMouseExit()
    {
        this.transform.DOLocalMoveY(0f, 0f);
    }

    private void OnMouseDown()
    {
        if (this.owner != FindObjectOfType<TurnHandler>().GetComponent<TurnHandler>().currentTurn)
        {
            return;
        }
        _spawnablesManager.SpawnTroop(this.gameObject);
    }

}
