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
    LevelController _levelController;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    private void Start()
    {
        cell = LayerMask.GetMask("Cell");
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cell) && !_levelController.GetAnyPopupEnabled())
        {
            
            if (_turnHandler.currentTurn == PlayerType.Player && hit.collider.GetComponent<CellBehaviour>().GetOwner() == PlayerType.Player)
            {
                lastCellSelected = hit.collider.GetComponent<CellBehaviour>();
                hit.collider.transform.DOLocalMoveY(0.5f, 0.3f);
            }
            
            if (_spawnablesManager.GetCurrentTroop() != TROOP.None 
                && _turnHandler.currentTurn == PlayerType.Player 
                && hit.collider.GetComponent<CellBehaviour>().GetOwner() == PlayerType.Player
                && Input.GetMouseButtonDown(0) 
                && lastCellSelected.GetTroopIn() == null 
                && _levelController.CheckIfCanSpawn()) //ningun NPC nuestro spawneado esta en estado ataque o move
            {
                _spawnablesManager.SpawnTroop(hit.collider.gameObject, Entity.Owner.Player);
            }

            if (_levelController.TryingToMove() != null)
            {
                bool nodeAccesible = _levelController.TryingToMove().gameObject.GetComponent<Troop>().ListPossibleMovementsContains(hit.collider.GetComponent<CellBehaviour>().PNode);
                Debug.Log("this node is accesible: " + nodeAccesible);
                if (Input.GetMouseButtonDown(0) 
                    && nodeAccesible)
                {
                    if (_levelController.TryingToMove().gameObject.GetComponent<Move>() != null){
                        _levelController.TryingToMove().gameObject.GetComponent<Move>().OnGoingCell = lastCellSelected;
                        _levelController.TryingToMove().gameObject.GetComponent<Move>().PathReceived = true;
                    }
                }
            }
        }

        if (lastCellSelected != null)
        {
            lastCellSelected.BackToInitialPosition();
        }
    }
}
