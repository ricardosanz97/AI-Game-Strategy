using System.Collections;
using System.Collections.Generic;
using StrategicAI;
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
    [Inject]private BloodController _bloodController;
    private LevelController _levelController;
    

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2.0f);

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

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

                Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "TU TURNO!");

                while (!playerDone)
                {
                    yield return null;
                }
                playerDone = false;
                currentTurn = PlayerType.None;

                if (_bloodController.PlayerBlood + _levelController.PlayerRewardBloodTurn > _bloodController.maxValue)
                {
                    _bloodController.SetPlayerBlood(_bloodController.maxValue);
                }
                else
                {
                    _bloodController.IncreasePlayerBloodValue(_levelController.PlayerRewardBloodTurn);
                }
                //TODO: ajustar

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
