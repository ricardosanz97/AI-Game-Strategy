using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TROOP {
    None,
    Minion,
    Archer,
    Tank,
    Wall,
    Construction
}
public class TroopController : MonoBehaviour {

	[SerializeField]private TROOP currentTroopSelected;

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
        switch (GameObject.FindObjectOfType<TroopController>().GetComponent<TroopController>().GetCurrentTroop())
        {
            case TROOP.Minion:
                Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Minion.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                break;
            case TROOP.Archer:
                Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Archer.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                break;
            case TROOP.Tank:
                Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Tank.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                break;
            case TROOP.Wall:
                Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Wall.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                break;
            case TROOP.Construction:
                Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + TROOP.Construction.ToString()), new Vector3(cell.transform.position.x, 0.6f, cell.transform.position.z), Quaternion.identity);
                break;
        }
    }

}
