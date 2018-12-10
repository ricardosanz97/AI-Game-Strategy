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
    Construction
}

public class SpawnablesManager : MonoBehaviour {

	[SerializeField]private TROOP currentTroopSelected;
    public TROOP lastTroopSpawned;

    public static event Action<Entity> OnSpawnedTroop;

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
        switch (currentTroopSelected)
        {
            case TROOP.None:
                Debug.Log("Antes debes seleccionar una tropa! ");
                FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().ShowButtons();
                break;
            case TROOP.Prisioner:
                lastTroopSpawned = TROOP.Prisioner;
                GameObject prisioner = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Prisioner.ToString());
                troopSpawned = Instantiate(prisioner, new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), prisioner.transform.rotation);
                currentTroopSelected = TROOP.None;
                break;
            case TROOP.Launcher:
                lastTroopSpawned = TROOP.Launcher;
                GameObject launcher = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Launcher.ToString());
                troopSpawned = Instantiate(launcher, new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), launcher.transform.rotation);
                currentTroopSelected = TROOP.None;
                break;
            case TROOP.Tank:
                lastTroopSpawned = TROOP.Tank;
                GameObject tank = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Tank.ToString());
                troopSpawned = Instantiate(tank, new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), tank.transform.rotation);
                currentTroopSelected = TROOP.None;
                break;
            case TROOP.Wall:
                lastTroopSpawned = TROOP.Wall;
                GameObject wall = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Wall.ToString());
                troopSpawned = Instantiate(wall, new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), wall.transform.rotation);
                break;
            case TROOP.Construction:
                lastTroopSpawned = TROOP.Construction;
                GameObject construction = Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Construction.ToString());
                troopSpawned = Instantiate(construction, new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), construction.transform.rotation);
                currentTroopSelected = TROOP.None;
                break;
        }

        if (troopSpawned != null)
        {
            OnSpawnedTroop?.Invoke(troopSpawned.GetComponent<Entity>());
            //todo asignar si el que spawnea es la ia o player
            FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().HideButtons();
            cell.GetComponent<CellBehaviour>().troopIn = troopSpawned.GetComponent<AbstractNPCBrain>();
        }
    }

}
