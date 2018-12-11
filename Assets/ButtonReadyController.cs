using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonReadyController : MonoBehaviour {

    [Inject]
    TurnHandler _turnHandler;
    public Color buttonReadyColor = Color.green;
    public Color otherTurnColor = Color.white;
    public Color buttonNotReadyColor = Color.red;

    private void Awake()
    {
        //this.GetComponent<Button>().colors.normalColor = buttonNotReadyColor;
    }

    public void ClickReady()
    {
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

        _turnHandler.playerDone = true;
        yield return null;
    }
}
