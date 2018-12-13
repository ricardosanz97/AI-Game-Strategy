using System;
using System.Collections;
using System.Collections.Generic;
using InfluenceMap;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public enum TROOP
{
    None,
    Prisioner,
    Launcher,
    Tank,
    Wall,
    Turret
}

public class SpawnablesManager : MonoBehaviour {

	[SerializeField]private TROOP currentTroopSelected;
    public TROOP lastTroopSpawned;
    public bool canSpawnTroop = true;
    public bool selectingWhereToMove = false;
    public bool selectingWhereToAttack = false;

    public static event Action<Entity> OnSpawnedTroop; 

    [Inject]
    BloodIndicatorController _bloodIndicatorController;

    LevelController _levelController;

    [Inject] private InfluenceMapComponent _influenceMapComponent;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    public void SetCurrentTroop(TROOP troop)
    {
        currentTroopSelected = troop;
    }

    public TROOP GetCurrentTroop()
    {
        return currentTroopSelected;
    }

    public void SpawnTroop(GameObject cell, Entity.Owner owner)
    {
        if (!_levelController.CheckIfCanSpawn())
        {
            return;
        }

        GameObject troopSpawned = null;
        GameObject troop = null;
        switch (currentTroopSelected)
        {
            case TROOP.None:
                Debug.Log("Antes debes seleccionar una tropa! ");
                FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().ShowButtons();
                break;
            case TROOP.Prisioner:
                lastTroopSpawned = TROOP.Prisioner;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Prisioner.ToString() + owner.ToString());
                break;
            case TROOP.Launcher:
                lastTroopSpawned = TROOP.Launcher;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Launcher.ToString() + owner.ToString());
                break;
            case TROOP.Tank:
                lastTroopSpawned = TROOP.Tank;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Tank.ToString() + owner.ToString());
                break;
            case TROOP.Wall:
                lastTroopSpawned = TROOP.Wall;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Wall.ToString() + owner.ToString());
                break;
            case TROOP.Turret:
                lastTroopSpawned = TROOP.Turret;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Turret.ToString() + owner.ToString());
                break;
        }

        if (troop != null && troop.GetComponent<Entity>().bloodCost <= _bloodIndicatorController.GetCurrentBlood())
        {
            troopSpawned = Instantiate(troop, new Vector3(cell.transform.position.x, 0f, cell.transform.position.z), troop.transform.rotation);
            troopSpawned.GetComponent<Entity>().SetEntity(owner);
            troopSpawned.GetComponent<AbstractNPCBrain>().npc = lastTroopSpawned;
            currentTroopSelected = TROOP.None;

            Node node = _influenceMapComponent.GetNodeAtLocation(new Vector3(cell.transform.position.x, 1f, cell.transform.position.z));
            print("spawned at: " + node.WorldGameObject.GetComponent<InfluencePosition>().GridPositions[0] + ", " + node.WorldGameObject.GetComponent<InfluencePosition>().GridPositions[1]);
        
            OnSpawnedTroop?.Invoke(troopSpawned.GetComponent<Entity>());
            _bloodIndicatorController.DecreaseBloodValue(troopSpawned.GetComponent<Entity>().bloodCost);
            FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().HideButtons();
            cell.GetComponent<CellBehaviour>().troopIn = troopSpawned.GetComponent<AbstractNPCBrain>();
            troopSpawned.GetComponent<AbstractNPCBrain>().cell = cell.GetComponent<CellBehaviour>();
        }
        else
        {
            return;
        }
    }

}
