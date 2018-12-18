using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonReadyController : MonoBehaviour {

    [Inject]
    TurnHandler _turnHandler;
    LevelController _levelController;
    public Color buttonReadyColor = Color.green;
    public Color otherTurnColor = Color.white;
    public Color buttonNotReadyColor = Color.red;
    private Button button;
    [Inject] private SoundManager soundManager;

    private void Awake()
    {
        button = this.gameObject.GetComponent<Button>();
        _levelController = FindObjectOfType<LevelController>();
        //this.GetComponent<Button>().colors.normalColor = buttonNotReadyColor;
    }

    private void Update()
    {
        if (_turnHandler.currentTurn != PlayerType.Player)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
    public void ClickReady()
    {
        //soundManager.PlaySingle(soundManager.readySound);//TODO: descomentar
        StartCoroutine(WaitForNPCsReady());
        //this.GetComponent<Button>().colors.normalColor = buttonReadyColor;
    }

    IEnumerator WaitForNPCsReady()
    {
        //check if all the players are ready
        //while (!allReady)
        //{
        //check if all the players are ready
        //yield return null;
        //}
        //this.GetComponent<Button>().colors.normalColor = buttonReadyColor;

        foreach (Entity e in _levelController.PlayerEntities)
        {
            if (_levelController.TryingToMove())
            {
                e.GetComponent<IdleOrder>().Idle = true;
            }
        }

        _turnHandler.playerDone = true;
        yield return null;
    }
}
