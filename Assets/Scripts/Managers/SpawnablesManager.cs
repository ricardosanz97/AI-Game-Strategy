using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum TROOP {
    None,
    Minion,
    Archer,
    Tank,
    Wall,
    Construction
}
public class SpawnablesManager : MonoBehaviour {

	[SerializeField]private TROOP currentTroopSelected;
    public TROOP lastTroopSpawned;

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
            case TROOP.Minion:
                lastTroopSpawned = TROOP.Minion;
                troopSpawned = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Minion.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                currentTroopSelected = TROOP.None;
                break;
            case TROOP.Archer:
                lastTroopSpawned = TROOP.Archer;
                troopSpawned = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Archer.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                currentTroopSelected = TROOP.None;
                break;
            case TROOP.Tank:
                lastTroopSpawned = TROOP.Tank;
                troopSpawned = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Tank.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                currentTroopSelected = TROOP.None;
                break;
            case TROOP.Wall:
                lastTroopSpawned = TROOP.Wall;
                troopSpawned = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Wall.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                break;
            case TROOP.Construction:
                troopSpawned = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Construction.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                lastTroopSpawned = TROOP.Construction;
                currentTroopSelected = TROOP.None;
                break;
        }

        if (troopSpawned != null)
        {
            Entity.OnTroopSpawned?.Invoke();
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleOptionsPopup"), cell.transform.position, Quaternion.identity);
                go.GetComponent<SimpleOptionsPopupController>().SetPopup(
                lastTroopSpawned.ToString(),
                "Mover",
                "Atacar",
                () => {
                    Debug.Log("MOVER");
                    go.GetComponent<SimpleOptionsPopupController>().ClosePopup();
                      },
                () => { Debug.Log("ATACAR");
                    go.GetComponent<SimpleOptionsPopupController>().ClosePopup(); });

            FindObjectOfType<AttackButtonController>().GetComponent<AttackButtonController>().HideButtons();
            cell.GetComponent<CellBehaviour>().troopIn = troopSpawned.GetComponent<AbstracNPCBrain>();
        }
    }

}
