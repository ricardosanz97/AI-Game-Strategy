using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialNPCPhrasePopupController : PopupController {

    SpawnablesManager _spawnablesManager;
    public Text initialPhrase;
    public string archer = "I will kill \nthem with \nmy arrows! ";
    public string minion = "Gweaa! \nCome here and \nfight me! ";
    public string tank = "ME! \nDESTROY! \nTOWER!";

    private void OnEnable()
    {
        switch (_spawnablesManager.lastTroopSpawned)
        {
            case TROOP.Minion:
                initialPhrase.text = minion;
                break;
            case TROOP.Archer:
                initialPhrase.text = archer;
                break;
            case TROOP.Tank:
                initialPhrase.text = tank;
                break;
        }
    }



}
