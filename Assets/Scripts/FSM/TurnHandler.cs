﻿using System.Collections;
using System.Collections.Generic;
using AI.StrategicAI;
using UnityEngine;
using Zenject;

public enum PlayerType
{
    None,
    Player,
    AI
}

public class TurnHandler : MonoBehaviour {

    public PlayerType currentTurn = PlayerType.None;
    public PlayerType lastTurn = PlayerType.None;
    public float WaitingTime = 2.0f;
    public bool playerDone = false;
    public bool AIDone = false;
    
    private bool isGameFinished = false;
    [Inject]private HighLevelAI _globalHighLevelAi;
    

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2.0f);

	void Start () {
    
        StartCoroutine(StartGame());
        StartCoroutine(HandleTurn());

	}

    IEnumerator StartGame()
    {
        yield return _waitForSeconds;
        currentTurn = Random.value > .5f ? PlayerType.Player : PlayerType.AI;
    }

    IEnumerator HandleTurn()
    {
        while (!isGameFinished)
        {
            while (currentTurn == PlayerType.None)
            {
                yield return null;
            }

            if (currentTurn == PlayerType.Player)
            {
                Debug.Log("TIRA EL PLAYER!");
                while (!playerDone)
                {
                    yield return null;
                }
                playerDone = false;
                currentTurn = PlayerType.None;
                yield return _waitForSeconds;
                currentTurn = PlayerType.AI;
            }

            else if (currentTurn == PlayerType.AI)
            {
                _globalHighLevelAi.PlayTurn();
                Debug.Log("TIRA LA IA");
                while (!AIDone)
                {
                    yield return null;
                }
                AIDone = false;
                currentTurn = PlayerType.None;
                yield return _waitForSeconds;
                currentTurn = PlayerType.Player;
            }

            yield return null;
        }
    }
}
