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

	[SerializeField]private ENTITY currentTroopSelected;
    public ENTITY lastTroopSpawned;
    public bool canSpawnTroop = true;
    public bool selectingWhereToMove = false;
    public bool selectingWhereToAttack = false;

    public static event Action<Entity> OnSpawnedTroop; 

    [Inject]
    BloodIndicatorController _bloodIndicatorController;

    LevelController _levelController;

    [Inject] private InfluenceMapComponent _influenceMapComponent;

    [Inject]
    private SoundManager soundManagerRef;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    public void SetCurrentTroop(ENTITY troop)
    {
        currentTroopSelected = troop;
    }

    public ENTITY GetCurrentTroop()
    {
        return currentTroopSelected;
    }

    public void SpawnTroopPlayer(GameObject cell, Entity.Owner owner)
    {
        if (!_levelController.CheckIfCanSpawn())
        {
            return;
        }

        GameObject troopSpawned = null;
        GameObject troop = null;
        
        switch (currentTroopSelected)
        {
            case ENTITY.None:
                Debug.Log("Antes debes seleccionar una tropa! ");
                FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().ShowButtons();
                soundManagerRef.PlaySingle(soundManagerRef.incorrectMovement);
                break;
            case ENTITY.Prisioner:
                lastTroopSpawned = ENTITY.Prisioner;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Prisioner.ToString() + owner.ToString());
                soundManagerRef.PlaySingle(soundManagerRef.cageSoundSpawn);
                break;
            case ENTITY.Launcher:
                lastTroopSpawned = ENTITY.Launcher;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Launcher.ToString() + owner.ToString());
                soundManagerRef.PlaySingle(soundManagerRef.launcherSoundSpawn);
                break;
            case ENTITY.Tank:
                lastTroopSpawned = ENTITY.Tank;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Tank.ToString() + owner.ToString());
                soundManagerRef.PlaySingle(soundManagerRef.tankSoundSpawn);
                break;
            case ENTITY.Wall:
                lastTroopSpawned = ENTITY.Wall;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Wall.ToString() + owner.ToString());
                break;
            case ENTITY.Turret:
                lastTroopSpawned = ENTITY.Turret;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + ENTITY.Turret.ToString() + owner.ToString());
                soundManagerRef.PlaySingle(soundManagerRef.turretSoundSpawn);
                break;
        }

        if (troop != null && troop.GetComponent<Entity>().bloodCost <= _bloodIndicatorController.GetCurrentBlood())
        {
            troopSpawned = Instantiate(troop, new Vector3(cell.transform.position.x, 0f, cell.transform.position.z), troop.transform.rotation);
            troopSpawned.GetComponent<Entity>().SetEntity(owner);
            troopSpawned.GetComponent<AbstractNPCBrain>().entityType = lastTroopSpawned;
            currentTroopSelected = ENTITY.None;

            Node node = _influenceMapComponent.GetNodeAtLocation(new Vector3(cell.transform.position.x, 1f, cell.transform.position.z));
            print("spawned at: " + node.WorldGameObject.GetComponent<InfluencePosition>().GridPositions[0] + ", " + node.WorldGameObject.GetComponent<InfluencePosition>().GridPositions[1]);
        
            OnSpawnedTroop?.Invoke(troopSpawned.GetComponent<Entity>());
            _bloodIndicatorController.DecreaseBloodValue(troopSpawned.GetComponent<Entity>().bloodCost);
            FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().HideButtons();
            cell.GetComponent<CellBehaviour>().entityIn = troopSpawned.GetComponent<AbstractNPCBrain>();
            troopSpawned.GetComponent<AbstractNPCBrain>().cell = cell.GetComponent<CellBehaviour>();
        }
        else
        {
            return;
        }
        
    }

    public void SpawnEntityAI(ENTITY troop, CellBehaviour cellBehaviour)
    {

        Entity entityToSpawn = null;
        Entity.Owner owner = Entity.Owner.AI;
        
        switch (troop)
        {
            case ENTITY.Prisioner:
                entityToSpawn = Resources.Load<Entity>("Prefabs/Enemies/" + ENTITY.Prisioner.ToString() + owner.ToString());
                break;
            case ENTITY.Launcher:
                entityToSpawn = Resources.Load<Entity>("Prefabs/Enemies/" + ENTITY.Launcher.ToString() + owner.ToString());
                break;
            case ENTITY.Tank:
                entityToSpawn = Resources.Load<Entity>("Prefabs/Enemies/" + ENTITY.Tank.ToString() + owner.ToString());
                break;
            case ENTITY.Wall:
                entityToSpawn = Resources.Load<Entity>("Prefabs/Enemies/" + ENTITY.Wall.ToString() + owner.ToString());
                break;
            case ENTITY.Turret:
                entityToSpawn = Resources.Load<Entity>("Prefabs/Enemies/" + ENTITY.Turret.ToString() + owner.ToString());
                break;
        }
        
        Assert.IsNotNull(entityToSpawn);
        entityToSpawn.owner = Entity.Owner.AI;

        Entity spawned = Instantiate(entityToSpawn, new Vector3(cellBehaviour.transform.position.x, 0f, cellBehaviour.transform.position.z), entityToSpawn.transform.rotation);
        spawned.owner = Entity.Owner.AI;

       
    
        OnSpawnedTroop?.Invoke(spawned);
        cellBehaviour.entityIn = spawned.GetComponent<AbstractNPCBrain>();
        spawned.GetComponent<AbstractNPCBrain>().cell = cellBehaviour;
        
    }

}
