using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class CellBehaviour : MonoBehaviour
{
    [Inject]
    private SpawnablesManager _spawnablesManager;

    private void OnMouseOver()
    {
        this.transform.DOLocalMoveY(0.5f, 0f);
    }

    private void OnMouseExit()
    {
        this.transform.DOLocalMoveY(0f, 0f);
    }

    private void OnMouseDown()
    {
        _spawnablesManager.SpawnTroop(this.gameObject);
    }

}
