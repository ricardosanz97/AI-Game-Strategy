using System.Collections;
using System.Linq;
using Boo.Lang;
using CustomPathfinding;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;

public class LevelController : MonoBehaviour {

    public GameObject rightSideCells;
    public GameObject leftSideCells;
    private GameObject canvasGameObject;
    public System.Collections.Generic.List<Entity> PlayerEntities;
    public System.Collections.Generic.List<Entity> AIEntities;
    public System.Collections.Generic.List<Entity> TotalEntities;
    public System.Collections.Generic.List<CustomPathfinding.Node> chosenNodesToMoveIA;
    public int MaxWalls = 8;
    public System.Collections.Generic.List<Entity> playerCoreEntities;
    public System.Collections.Generic.List<Entity> AICoreEntities;

    public int MinBloodReward = 10;

    public int PlayerRewardBloodTurn = 10;
    public int AIRewardBloodTurn = 10;

    public int MaxTroopsSpawned = 5;
    public int currentTroopsPlayerSpawned = 0;
    public int currentTroopsAISpawned = 0;

    public Slider playerCoreHPSlider; //POR INSPECTOR
    public Slider AICoreHPSlider; //POR INSPECTOR
    public int PlayerCoreHP;
    public int AICoreHP;
    private bool pauseMenuEnabled = true;
    private SoundManager soundManager;

    public SpawnableCell[] cellsSpawnables;

    private void OnEnable()
    {
        Entity.OnTroopDeleted += OnEntityDeleted;
    }

    private void OnDisable()
    {
        Entity.OnTroopDeleted -= OnEntityDeleted;
    }

    private void Awake()
    {
        canvasGameObject = FindObjectOfType<Canvas>().gameObject;
        soundManager = FindObjectOfType<SoundManager>();
        chosenNodesToMoveIA = new System.Collections.Generic.List<Node>();
    }

    private void Start()
    {
        cellsSpawnables = leftSideCells.transform.GetComponentsInChildren<SpawnableCell>(); 
        //ResetAllShadersCells();
        AICoreEntities = GameObject.FindGameObjectWithTag("CoreAI").GetComponentsInChildren<Entity>().ToList();
        playerCoreEntities = GameObject.FindGameObjectWithTag("CorePlayer").GetComponentsInChildren<Entity>().ToList();

        PlayerCoreHP = playerCoreEntities.Count;
        AICoreHP = AICoreEntities.Count;

        playerCoreHPSlider.maxValue = PlayerCoreHP;
        playerCoreHPSlider.value = playerCoreHPSlider.maxValue;

        AICoreHPSlider.maxValue = AICoreHP;
        AICoreHPSlider.value = AICoreHPSlider.maxValue;

        LayerMask mask = LayerMask.GetMask(new string[] { "Cell" });
        foreach (var entity in AICoreEntities)
        {
            Collider[] cols = Physics.OverlapBox(entity.transform.position, new Vector3(0f, 1f, 0f), Quaternion.identity, mask);
            foreach (Collider c in cols)
            {
                if (c.GetComponent<CellBehaviour>() != null)
                {
                    entity.cell = c.GetComponent<CellBehaviour>();
                    c.GetComponent<CellBehaviour>().entityIn = entity;
                }
            }
        }

        foreach (var entity in playerCoreEntities)
        {
            Collider[] cols = Physics.OverlapBox(entity.transform.position, new Vector3(0f, 1f, 0f), Quaternion.identity, mask);
            foreach (Collider c in cols)
            {
                if (c.GetComponent<CellBehaviour>() != null)
                {
                    entity.cell = c.GetComponent<CellBehaviour>();
                    c.GetComponent<CellBehaviour>().entityIn = entity;
                }
            }
        }

        StartCoroutine(CheckWetherIsAWinner());

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

    IEnumerator CheckWetherIsAWinner()
    {
        while (true)
        {
            if (PlayerCoreHP <= 0)
            {
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "YOU LOSE!");
                yield return new WaitForSeconds(3f);
                SceneManager.LoadScene(0);
            }
            else if (AICoreHP <= 0)
            {
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Popups/SimpleInfoPopup")).GetComponent<SimpleInfoPopupController>().SetPopup("PLAYER", "YOU WIN!");
                yield return new WaitForSeconds(3f);
                SceneManager.LoadScene(0);
            }
            yield return null;
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

    public void EnableSpawnableCellsShader()
    {
        foreach (SpawnableCell sc in cellsSpawnables)
        {
            if(sc.GetComponent<CellBehaviour>().entityIn == null)
            {
                sc.transform.Find("SpawnPlacement").gameObject.SetActive(true);
            }
        }
    }

    public void DisableSpawnableCellsShader()
    {
        foreach (SpawnableCell sc in cellsSpawnables)
        {
            sc.transform.Find("SpawnPlacement").gameObject.SetActive(false);
        }
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
        else if (owner == Entity.Owner.AI)
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
                if (e.GetComponent<Troop>() != null)
                {
                    e.GetComponent<Troop>().DisableShaderAttackCells();
                    e.GetComponent<Troop>().DisableShaderMoveCells();

                    //e.GetComponent<IdleOrder>().Idle = true;
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
                if (e.GetComponent<Troop>() != null)
                {
                    e.GetComponent<Troop>().DisableShaderAttackCells();
                    e.GetComponent<Troop>().DisableShaderMoveCells();

                    //e.GetComponent<IdleOrder>().Idle = true;
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
    
    public void RemovePlayerEntities(Entity entity)
    {
        PlayerEntities.Remove(entity);
        TotalEntities.Remove(entity);
        currentTroopsPlayerSpawned -= 1;
    }

    public void RemoveAIEntities(Entity entity)
    {
        AIEntities.Remove(entity);
        TotalEntities.Remove(entity);  
        currentTroopsAISpawned -= 1;

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

    private void OnEntityDeleted(Entity entityDeleted)
    {
        if(entityDeleted.owner == Entity.Owner.Player)
        {
            if(entityDeleted.entityType == ENTITY.Core)
            {
                if (playerCoreEntities.Contains(entityDeleted))
                {
                    playerCoreEntities.Remove(entityDeleted);
                    PlayerCoreHP--;
                    playerCoreHPSlider.value--;
                }
                    
            }
            else if(PlayerEntities.Contains(entityDeleted))
               RemovePlayerEntities(entityDeleted);
        }
        else if(entityDeleted.owner == Entity.Owner.AI)
        {
            if(entityDeleted.entityType == ENTITY.Core)
            {
                if (AICoreEntities.Contains(entityDeleted))
                {
                    AICoreEntities.Remove(entityDeleted);
                    AICoreHP--;
                    AICoreHPSlider.value--;
                }
                    
            }
            else if(AIEntities.Contains(entityDeleted))
                RemoveAIEntities(entityDeleted);
        }
    }

}
