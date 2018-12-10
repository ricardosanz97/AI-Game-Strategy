using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialNPCPhrasePopupController : PopupController {

    SpawnablesManager _spawnablesManager;
    public Text initialPhrase;
    public string launcher = "I will kill \nthem with \nmy arrows! ";
    public string prisioner = "Gweaa! \nCome here and \nfight me! ";
    public string tank = "ME! \nDESTROY! \nTOWER!";

    private void OnEnable()
    {
        switch (_spawnablesManager.lastTroopSpawned)
        {
            case TROOP.Prisioner:
                initialPhrase.text = prisioner;
                break;
            case TROOP.Launcher:
                initialPhrase.text = launcher;
                break;
            case TROOP.Tank:
                initialPhrase.text = tank;
                break;
        }
    }



}
