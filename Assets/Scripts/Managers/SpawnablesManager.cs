using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using InfluenceMap;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Zenject.Asteroids;

public enum ENTITY
{
    None,
    Prisioner,
    Launcher,
    Tank,
    Wall,
    Turret,
    Core
}

public class SpawnablesManager : MonoBehaviour {

	[SerializeField]public ENTITY currentEntitySelected;

    public ENTITY lastTroopSpawned;
    public bool canSpawnTroop = true;
    public bool selectingWhereToMove = false;
    public bool selectingWhereToAttack = false;

    public static event Action<Entity> OnSpawnedTroop; 

    [Inject]
    BloodController _bloodController;

    LevelController _levelController;

    [Inject] private InfluenceMapComponent _influenceMapComponent;

    [Inject]
    private SoundManager soundManagerRef;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    public void SetCurrentEntity(ENTITY troop)
    {
        if (troop != ENTITY.None)
        {
            _levelController.EnableSpawnableCellsShader();
        }
        currentEntitySelected = troop;
    }

    public ENTITY GetCurrentEntity()
    {
        return currentEntitySelected;
    }

    public Entity GetEntity(ENTITY entity)
    {
        return Resources.Load<GameObject>("Prefabs/Enemies/" + currentEntitySelected.ToString() + "Player").GetComponent<Entity>();
    }

    public void SpawnEntity(CellBehaviour cell, ENTITY _entityToSpawn, Entity.Owner owner)
    {
        if (!_levelController.CheckIfCanSpawn(owner))
        {
            return;
        }
        
        GameObject entitySpawned = null;
        GameObject entityToSpawn = null;
        AudioClip entitySpawnSound = null;
        switch (_entityToSpawn)
        {
            case ENTITY.None:
                Debug.Log("Antes debes seleccionar una tropa! ");
                FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().ShowButtons();
                soundManagerRef.PlaySingle(soundManagerRef.incorrectMovement);
                break;
            case ENTITY.Prisioner:
                lastTroopSpawned = ENTITY.Prisioner;
                entityToSpawn = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Prisioner.ToString() + owner.ToString());
                entitySpawnSound = soundManagerRef.cageSoundSpawn;
                break;
            case ENTITY.Launcher:
                lastTroopSpawned = ENTITY.Launcher;
                entityToSpawn = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Launcher.ToString() + owner.ToString());
                entitySpawnSound = soundManagerRef.launcherSoundSpawn;
                break;
            case ENTITY.Tank:
                lastTroopSpawned = ENTITY.Tank;
                entityToSpawn = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Tank.ToString() + owner.ToString());
                entitySpawnSound = soundManagerRef.tankSoundSpawn;
                break;
            case ENTITY.Wall:
                lastTroopSpawned = ENTITY.Wall;
                entityToSpawn = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Wall.ToString() + owner.ToString());
                break;
            case ENTITY.Turret:
                lastTroopSpawned = ENTITY.Turret;
                entityToSpawn = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Turret.ToString() + owner.ToString());
                entitySpawnSound = soundManagerRef.turretSoundSpawn;
                break;
        }

        if (entityToSpawn == null)
        {
            return;
        }
        int bloodCost = entityToSpawn.GetComponent<Entity>().bloodCost;
        bool bloodEnough = owner == Entity.Owner.Player ? bloodCost <= _bloodController.GetCurrentPlayerBlood() : bloodCost <= _bloodController.GetCurrentAIBlood();

        if (bloodEnough)
        {
            if (entityToSpawn != null)
            {
                soundManagerRef.PlaySingle(entitySpawnSound);
                entitySpawned = Instantiate(entityToSpawn, new Vector3(cell.transform.position.x, 0f, cell.transform.position.z), entityToSpawn.transform.rotation);
                entitySpawned.GetComponent<Entity>().SetEntity(owner);
                entitySpawned.GetComponent<Entity>().entityType = lastTroopSpawned;

                if (owner == Entity.Owner.Player)
                    currentEntitySelected = ENTITY.None;

                Node node = _influenceMapComponent.GetNodeAtLocation(new Vector3(cell.transform.position.x, 1f, cell.transform.position.z));

                OnSpawnedTroop?.Invoke(entitySpawned.GetComponent<Entity>());

                if (owner == Entity.Owner.Player)
                {
                    _levelController.currentTroopsPlayerSpawned++;
                    _bloodController.DecreasePlayerBloodValue(entitySpawned.GetComponent<Entity>().bloodCost);
                }
                else if (owner == Entity.Owner.AI)
                {
                    Debug.Log("decreasing blood");
                    _levelController.currentTroopsAISpawned++;
                    _bloodController.DecreaseAIBloodValue(entitySpawned.GetComponent<Entity>().bloodCost);
                }

                FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().HideButtons();
                cell.GetComponent<CellBehaviour>().entityIn = entitySpawned.GetComponent<AbstractNPCBrain>();
                entitySpawned.GetComponent<Entity>().cell = cell.GetComponent<CellBehaviour>();
            }
        }

        else
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "NOT ENOUGH\nBLOOD");
        }
        
    }

}
