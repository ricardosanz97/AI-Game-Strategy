using CustomPathfinding;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class RaycastCamera : MonoBehaviour {
    Ray ray;
    RaycastHit hit;
    LayerMask cell;
    [Inject]
    SpawnablesManager _spawnablesManager;
    BloodController _bloodController;
    [Inject]
    TurnHandler _turnHandler;
    CellBehaviour lastCellSelected;
    LevelController _levelController;
    [Inject]
    SoundManager soundManager;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
        _bloodController = FindObjectOfType<BloodController>();
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
            EffectCell();

            SpawnIfPossibleInCell();

            MoveIfSomeoneIsTryingTo();

            AttackIfSomeoneIsTryingTo();
            
        }

        if (lastCellSelected != null)
        {
            lastCellSelected.BackToInitialPosition();
        }
    }

    private void SpawnIfPossibleInCell()
    {
        if (_turnHandler.currentTurn == PlayerType.Player
                && hit.collider.GetComponent<CellBehaviour>().GetOwner() == PlayerType.Player
                && Input.GetMouseButtonDown(0)
                && lastCellSelected.GetEntityIn() == null
                && _levelController.CheckIfCanSpawn(Entity.Owner.Player) && hit.collider.GetComponent<CellBehaviour>().GetComponent<SpawnableCell>() != null) //ningun NPC nuestro spawneado esta en estado ataque o move
        {
            _spawnablesManager.SpawnEntity(hit.collider.GetComponent<CellBehaviour>(), _spawnablesManager.currentEntitySelected, Entity.Owner.Player);
        }
    }

    private void MoveIfSomeoneIsTryingTo()
    {
        if (_levelController.TryingToMove() != null)
        {
            if (hit.collider.GetComponent<CellBehaviour>().entityIn == _levelController.TryingToMove() && Input.GetMouseButtonDown(0))
            {
                hit.collider.GetComponent<CellBehaviour>().entityIn.GetComponent<IdleOrder>().Idle = true; //cancelamos.
                soundManager.PlaySingle(soundManager.cancelActionSound);
            }

            bool nodeMovementAccesible = _levelController.TryingToMove().gameObject.GetComponent<Troop>().ListPossibleMovementsContains(hit.collider.GetComponent<CellBehaviour>().PNode);
            if (Input.GetMouseButtonDown(0)
                && nodeMovementAccesible)
            {
                Debug.Log("CLICK IN A CELL TO MOVE. ");
                if (_levelController.TryingToMove().gameObject.GetComponent<Move>() != null)
                {
                    bool bloodEnough = _bloodController.GetCurrentPlayerBlood() >= _levelController.TryingToMove().gameObject.GetComponent<Attack>().bloodCost;
                    if (bloodEnough)
                    {
                        _bloodController.DecreasePlayerBloodValue(_levelController.TryingToMove().GetComponent<Move>().bloodCost);
                        soundManager.PlaySingle(soundManager.buttonPressedSound);
                        _levelController.TryingToMove().gameObject.GetComponent<Move>().OnGoingCell = lastCellSelected;
                        _levelController.TryingToMove().gameObject.GetComponent<Move>().PathReceived = true;
                    }
                    else
                    {
                        Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "NOT ENOUGH\nBLOOD");
                    }
                }
            }
        }
    }

    private void AttackIfSomeoneIsTryingTo()
    {
        if (_levelController.TryingToAttack() != null)
        {
            if (_levelController.TryingToAttack().gameObject.GetComponent<Troop>() != null) //si esta en estado atacar pero no es la torre (que siempre esta en ataque atacar).
            {
                if (hit.collider.GetComponent<CellBehaviour>().entityIn == _levelController.TryingToAttack() && Input.GetMouseButtonDown(0))
                {
                    hit.collider.GetComponent<CellBehaviour>().entityIn.GetComponent<IdleOrder>().Idle = true;
                    soundManager.PlaySingle(soundManager.cancelActionSound);
                }

                bool nodeAttackAccesible = _levelController.TryingToAttack().gameObject.GetComponent<Troop>().ListPossibleAttacksContains(hit.collider.GetComponent<CellBehaviour>().PNode);
                
                if (Input.GetMouseButtonDown(0)
                    && nodeAttackAccesible)
                {
                    Debug.Log("CLICK IN A CELL TO ATTACK. ");
                    if (_levelController.TryingToAttack().gameObject.GetComponent<Attack>() != null)
                    {
                        bool bloodEnough = _bloodController.GetCurrentPlayerBlood() >= _levelController.TryingToAttack().gameObject.GetComponent<Attack>().bloodCost;
                        if (bloodEnough)
                        {
                            _bloodController.DecreasePlayerBloodValue(_levelController.TryingToAttack().GetComponent<Attack>().bloodCost);
                            _levelController.TryingToAttack().gameObject.GetComponent<AbstractNPCBrain>().DoAttackAnimation();
                            _levelController.TryingToAttack().gameObject.GetComponent<Attack>().targetEntity = lastCellSelected.entityIn;
                            _levelController.TryingToAttack().gameObject.GetComponent<Attack>().ObjectiveAssigned = true;
                        }

                        else
                        {
                            Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "NOT ENOUGH\nBLOOD");
                        }
                    }
                }
            }
        }
    }

    private void EffectCell()
    {
        if (_turnHandler.currentTurn == PlayerType.Player)
        {
            lastCellSelected = hit.collider.GetComponent<CellBehaviour>();
            if (hit.collider.GetComponent<CellBehaviour>().GetOwner() == PlayerType.Player)
            {
                hit.collider.transform.DOLocalMoveY(0.5f, 0.3f);
            }
        }
    }
}
