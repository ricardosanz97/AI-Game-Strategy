using UnityEngine;
using DG.Tweening;
using Zenject;

public class RaycastCamera : MonoBehaviour {
    Ray ray;
    RaycastHit hit;
    LayerMask cell;
    [Inject]
    SpawnablesManager _spawnablesManager;
    [Inject]
    TurnHandler _turnHandler;
    CellBehaviour lastCellSelected;

    private void Start()
    {
        cell = LayerMask.GetMask("Cell");
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cell))
        {
            if (_spawnablesManager.GetCurrentTroop() == TROOP.None || _turnHandler.currentTurn != PlayerType.Player || hit.collider.GetComponent<CellBehaviour>().GetOwner() != PlayerType.Player) //si no hay tropa seleccionada o no es el turno del player o la casilla no es del player...
            {
                return;
            }
            lastCellSelected = hit.collider.GetComponent<CellBehaviour>();
            hit.collider.transform.DOLocalMoveY(0.5f, 0.3f);
            if (Input.GetMouseButtonDown(0) && lastCellSelected.GetTroopIn() == null)
            {
                _spawnablesManager.SpawnTroop(hit.collider.gameObject);
            }
        }

        if (lastCellSelected != null)
        {
            lastCellSelected.BackToInitialPosition();
        }
    }
}
