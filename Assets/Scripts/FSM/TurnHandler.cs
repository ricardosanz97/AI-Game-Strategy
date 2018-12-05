using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENTITY
{
    None,
    Player,
    AI
}

public class TurnHandler : MonoBehaviour {

    public ENTITY currentTurn = ENTITY.None;
    public ENTITY lastTurn = ENTITY.None;

    public bool playerDone = false;
    public bool AIDone = false;

	void Start () {
    
        StartCoroutine(StartGame());
        StartCoroutine(HandleTurn());

	}

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        currentTurn = currentTurn = Random.value > .5f ? ENTITY.Player : ENTITY.AI;
    }

    IEnumerator HandleTurn()
    {
        while (true)
        {
            while (currentTurn == ENTITY.None)
            {
                yield return null;
            }

            if (currentTurn == ENTITY.Player)
            {
                Debug.Log("TIRA EL PLAYER!");
                while (!playerDone)
                {
                    yield return null;
                }
                playerDone = false;
                currentTurn = ENTITY.None;
                yield return new WaitForSeconds(2f);
                currentTurn = ENTITY.AI;
            }

            else if (currentTurn == ENTITY.AI)
            {
                //AIManager.ExecutePlay();
                Debug.Log("TIRA LA IA");
                while (!AIDone)
                {
                    yield return null;
                }
                AIDone = false;
                currentTurn = ENTITY.None;
                yield return new WaitForSeconds(2f);
                currentTurn = ENTITY.Player;
            }

            yield return true;
        }
    }
}
