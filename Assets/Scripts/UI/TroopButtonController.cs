using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopButtonController : MonoBehaviour {

    public TROOP troopType;

    private void OnEnable()
    {
        
    }

    public void SelectTroop()
    {
        GameObject.FindObjectOfType<TroopController>().GetComponent<TroopController>().SetCurrentTroop(troopType);
    }
    

}
