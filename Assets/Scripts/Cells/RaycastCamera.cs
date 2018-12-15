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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cell) && ! _levelController.GetAnyPopupEnabled())
        {
            if (_turnHandler.currentTurn == PlayerType.Player && hit.collider.GetComponent<CellBehaviour>().GetOwner() == PlayerType.Player)
            {
                lastCellSelected = hit.collider.GetComponent<CellBehaviour>();
                hit.collider.transform.DOLocalMoveY(0.5f, 0.3f);
            }
            
            if (_turnHandler.currentTurn == PlayerType.Player
                && hit.collider.GetComponent<CellBehaviour>().GetOwner() == PlayerType.Player
                && Input.GetMouseButtonDown(0)
                && lastCellSelected.GetEntityIn() == null
                && _levelController.CheckIfCanSpawn()) //ningun NPC nuestro spawneado esta en estado ataque o move
            {
                _spawnablesManager.SpawnTroopPlayer(hit.collider.gameObject, Entity.Owner.Player);
            }

            if (_levelController.TryingToMove() != null)
            {
                bool nodeMovementAccesible = _levelController.TryingToMove().gameObject.GetComponent<Troop>().ListPossibleMovementsContains(hit.collider.GetComponent<CellBehaviour>().PNode);
                Debug.Log("this node is accesible: " + nodeMovementAccesible);
                if (Input.GetMouseButtonDown(0) 
                    && nodeMovementAccesible)
                {
                    if (_levelController.TryingToMove().gameObject.GetComponent<Move>() != null){
                        _levelController.TryingToMove().gameObject.GetComponent<Move>().OnGoingCell = lastCellSelected;
                        _levelController.TryingToMove().gameObject.GetComponent<Move>().PathReceived = true;
                    }
                }
            }
            else if (_levelController.TryingToAttack() != null)
            {
                if (_levelController.TryingToAttack().gameObject.GetComponent<Troop>() != null) //si esta en estado atacar pero no es la torre (que siempre esta en ataque atacar).
                {
                    bool nodeAttackAccesible = _levelController.TryingToAttack().gameObject.GetComponent<Troop>().ListPossibleAttacksContains(hit.collider.GetComponent<CellBehaviour>().PNode);
                    if (Input.GetMouseButtonDown(0)
                        && nodeAttackAccesible)
                    {
                        if (_levelController.TryingToAttack().gameObject.GetComponent<Attack>() != null)
                        {
                            _levelController.TryingToAttack().gameObject.GetComponent<AbstractNPCBrain>().DoAttackAnimation();
                            _levelController.TryingToAttack().gameObject.GetComponent<Attack>().NPCObjectiveAttack = lastCellSelected.entityIn.GetComponent<AbstractNPCBrain>();
                            _levelController.TryingToAttack().gameObject.GetComponent<Attack>().ObjectiveAssigned = true;

                        }
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
