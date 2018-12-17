using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour {

    public GameObject rightSideCells;
    public GameObject leftSideCells;
    private GameObject canvasGameObject;
    public List<Entity> PlayerEntities;
    public List<Entity> AIEntities;
    public List<Entity> TotalEntities;
    public List<CustomPathfinding.Node> chosenNodesToMoveIA;
    public int MaxWalls = 8;

    public List<Entity> playerCoreEntities;
    public List<Entity> AICoreEntities;

    public int MinBloodReward = 10;

    public int PlayerRewardBloodTurn = 10;
    public int AIRewardBloodTurn = 10;

    public int MaxTroopsSpawned = 5;
    public int currentTroopsPlayerSpawned = 0;
    public int currentTroopsAISpawned = 0;

    private bool pauseMenuEnabled = true;
    private SoundManager soundManager;

    private void Awake()
    {
        canvasGameObject = FindObjectOfType<Canvas>().gameObject;
        soundManager = FindObjectOfType<SoundManager>();
        chosenNodesToMoveIA = new List<Node>();
    }

    private void Start()
    {
        //ResetAllShadersCells();
        GameObject[] AIEntitiesGO = GameObject.FindGameObjectsWithTag("CoreAI");
        GameObject[] PlayerEntitiesGO = GameObject.FindGameObjectsWithTag("CorePlayer");
        LayerMask mask = LayerMask.GetMask(new string[] { "Cell" });
        foreach (var go in AIEntitiesGO)
        {
            AICoreEntities.Add(go.GetComponent<Entity>());
            Collider[] cols = Physics.OverlapBox(go.transform.position, new Vector3(0f, 1f, 0f), Quaternion.identity, mask);
            foreach (Collider c in cols)
            {
                if (c.GetComponent<CellBehaviour>() != null)
                {
                    go.GetComponent<Entity>().cell = c.GetComponent<CellBehaviour>();
                    c.GetComponent<CellBehaviour>().entityIn = go.GetComponent<Entity>();
                }
            }
        }

        foreach (var o in PlayerEntitiesGO)
        {
            playerCoreEntities.Add(o.GetComponent<Entity>());
            Collider[] cols = Physics.OverlapBox(o.transform.position, new Vector3(0f, 1f, 0f), Quaternion.identity, mask);
            foreach (Collider c in cols)
            {
                if (c.GetComponent<CellBehaviour>() != null)
                {
                    o.GetComponent<Entity>().cell = c.GetComponent<CellBehaviour>();
                    c.GetComponent<CellBehaviour>().entityIn = o.GetComponent<Entity>();
                }
            }
        }
    }

    private void Update()
    {
        if (pauseMenuEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenuEnabled = false;
                soundManager.PlayBackground(soundManager.gameBackgroundSound, soundManager.pauseVolume);
                GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimplePausePopup"));
                go.GetComponent<SimplePausePopupController>().SetPopup(
                Vector3.zero,
                "Pause",
                "Resume",
                "Quit",
                () => {
                    pauseMenuEnabled = true;
                    go.GetComponent<SimplePausePopupController>().ClosePopup();
                    soundManager.PlayBackground(soundManager.gameBackgroundSound, soundManager.defaultVolume);
                },
                () => {
                    pauseMenuEnabled = true;
                    go.GetComponent<SimplePausePopupController>().ClosePopup();
                    soundManager.PlaySingle(soundManager.quitSound);
                    soundManager.PlayBackground(soundManager.menuBackgroundSound, soundManager.defaultVolume);
                    SceneManager.LoadScene(0);
                },
                () =>
                {
                    pauseMenuEnabled = true;
                    go.GetComponent<SimplePausePopupController>().ClosePopup();
                    soundManager.PlayBackground(soundManager.gameBackgroundSound, soundManager.defaultVolume);
                });
            }
        }
        
    }

    void ResetAllShadersCells()
    {
        for (int i = 0; i < rightSideCells.transform.childCount; i++)
        {
            if (rightSideCells.transform.GetChild(i).transform.Find("ProjectilePlacement") != null)
            {
                rightSideCells.transform.GetChild(i).transform.Find("ProjectilePlacement").gameObject.SetActive(false);
            }

        }

        for (int i = 0; i < leftSideCells.transform.childCount; i++)
        {
            if (leftSideCells.transform.GetChild(i).transform.Find("ProjectilePlacement") != null)
            {
                leftSideCells.transform.GetChild(i).transform.Find("ProjectilePlacement").gameObject.SetActive(false);
            }
        }
    }

    public bool GetAnyPopupEnabled()
    {
        GameObject popupContainer = GameObject.FindGameObjectWithTag("PopupsContainer");
        if (popupContainer.transform.childCount > 0)
        {
            return true;
        }
        return false;
    }

    public void ClosePopups()
    {
        GameObject popupContainer = GameObject.FindGameObjectWithTag("PopupsContainer");
        if (popupContainer.transform.childCount > 0)
        {
            for (int i = 0; i<popupContainer.transform.childCount; i++)
            {
                popupContainer.transform.GetChild(i).GetComponent<PopupController>().ClosePopup();
            }
        }
    }

    public bool CheckIfCanSpawn(Entity.Owner owner) {
        bool can = false;
        if (owner == Entity.Owner.Player)
        {
            foreach (Entity entity in PlayerEntities)
            {
                if (entity.gameObject.GetComponent<Troop>() != null
                    && entity.gameObject.GetComponent<AbstractNPCBrain>().currentState != null
                    && entity.gameObject.GetComponent<AbstractNPCBrain>().currentState.stateName != STATE.Idle) //is moving or attacking
                {
                    can = false;
                }
            }
            if (currentTroopsPlayerSpawned >= MaxTroopsSpawned)
            {
                can = false;
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "TIENES\n5 TROPAS");
            }
            else
            {
                can = true;
            }
        }
        else
        {
            if (currentTroopsAISpawned >= MaxTroopsSpawned)
            {
                can = false;
            }
            else
            {
                can = true;
            }
        }

        return can;
        
    }

    public void ResetTroopParameters(PlayerType nextTurn)
    {
        if (nextTurn == PlayerType.AI)
        {
            foreach (Entity e in AIEntities)
            {
                if (e.GetComponent<AbstractNPCBrain>() != null)
                {
                    e.GetComponent<AbstractNPCBrain>().executed = false;
                    e.GetComponent<AbstractNPCBrain>().damageTurretReceived = false;
                }
                if (e.GetComponent<Entity>().entityType == ENTITY.Turret)
                {
                    e.GetComponent<TurretNPC>().SetAffectedCells();
                }
            }
            PlayerRewardBloodTurn = MinBloodReward;
        }
        else if (nextTurn == PlayerType.Player)
        {
            foreach (Entity e in PlayerEntities)
            {
                if (e.GetComponent<AbstractNPCBrain>() != null)
                {
                    e.GetComponent<AbstractNPCBrain>().executed = false;
                    e.GetComponent<AbstractNPCBrain>().damageTurretReceived = false;
                }
                if (e.GetComponent<Entity>().entityType == ENTITY.Turret)
                {
                    e.GetComponent<TurretNPC>().SetAffectedCells();
                }
            }
            AIRewardBloodTurn = MinBloodReward;
        }
    }

    public void AddPlayerEntities(Entity entity)
    {
        PlayerEntities.Add(entity);
        TotalEntities.Add(entity);
    }

    public void AddAIEntities(Entity entity)
    {
        AIEntities.Add(entity);
        TotalEntities.Add(entity);
    }

    public Entity TryingToMove()
    {
        foreach (Entity entity in PlayerEntities)
        {
            if (entity.gameObject.GetComponent<AbstractNPCBrain>() != null && entity.gameObject.GetComponent<AbstractNPCBrain>().currentState.stateName == STATE.Move && !entity.gameObject.GetComponent<Move>().moving)
            {
                return entity;
            }
        }
        return null;
    }

    public Entity TryingToAttack()
    {
        foreach (Entity entity in PlayerEntities)
        {
            if (entity.gameObject.GetComponent<AbstractNPCBrain>() != null && entity.gameObject.GetComponent<AbstractNPCBrain>().currentState.stateName == STATE.Attack)
            {
                return entity;
            }
        }
        return null;
    }


}
