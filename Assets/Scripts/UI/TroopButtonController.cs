using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TroopButtonController : MonoBehaviour {

    public TROOP troopType;
    
    [Inject]
    private SpawnablesManager _spawnablesManager;

    private LevelController _levelController;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>().GetComponent<LevelController>();
    }

    public void SelectTroop()
    {
        _levelController.ClosePopups();
        _spawnablesManager.SetCurrentTroop(troopType);
    }

}
