using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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

    public static event Action<Entity> OnSpawnedTroop;

    [Inject]
    BloodIndicatorController _bloodIndicatorController;

    public void SetCurrentTroop(TROOP troop)
    {
        currentTroopSelected = troop;
    }

    public TROOP GetCurrentTroop()
    {
        return currentTroopSelected;
    }

    public void SpawnTroop(GameObject cell)
    {
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
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Prisioner.ToString());
                break;
            case TROOP.Launcher:
                lastTroopSpawned = TROOP.Launcher;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Launcher.ToString());
                break;
            case TROOP.Tank:
                lastTroopSpawned = TROOP.Tank;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Tank.ToString());
                break;
            case TROOP.Wall:
                lastTroopSpawned = TROOP.Wall;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Wall.ToString());
                break;
            case TROOP.Turret:
                lastTroopSpawned = TROOP.Turret;
                troop = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Turret.ToString());
                break;
        }

        if (troop != null && troop.GetComponent<Entity>().bloodCost <= _bloodIndicatorController.GetCurrentBlood())
        {
            troopSpawned = Instantiate(troop, new Vector3(cell.transform.position.x, 1f, cell.transform.position.z), troop.transform.rotation);
            currentTroopSelected = TROOP.None;

            OnSpawnedTroop?.Invoke(troopSpawned.GetComponent<Entity>());
            //todo asignar si el que spawnea es la ia o player

            _bloodIndicatorController.DecreaseBloodValue(troopSpawned.GetComponent<Entity>().bloodCost);

            FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().HideButtons();
            cell.GetComponent<CellBehaviour>().troopIn = troopSpawned.GetComponent<AbstractNPCBrain>();
        }
        else
        {
            return;
        }
    }

}
