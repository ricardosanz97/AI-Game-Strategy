using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TroopButtonController : MonoBehaviour {

    public TROOP troopType;
    
    [Inject]
    private SpawnablesManager _spawnablesManager;

    private void Awake()
    {
        _spawnablesManager = FindObjectOfType<SpawnablesManager>().GetComponent<SpawnablesManager>();
    }

    public void SelectTroop()
    {
        _spawnablesManager.SetCurrentTroop(troopType);
    }

}
