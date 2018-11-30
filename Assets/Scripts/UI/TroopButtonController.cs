using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TroopButtonController : MonoBehaviour {

    public TROOP troopType;
    
    [Inject]
    private SpawnablesManager _spawnablesManager;

    public void SelectTroop()
    {
        _spawnablesManager.SetCurrentTroop(troopType);
    }
    

}
