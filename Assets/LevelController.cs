using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour {

    private GameObject canvasGameObject;
    public List<Entity> PlayerEntities;
    public List<Entity> AIEntities;
    public List<Entity> TotalEntities;

    private void Awake()
    {
        canvasGameObject = FindObjectOfType<Canvas>().gameObject;
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

    public bool CheckIfCanSpawn()
    {
        bool can = true;
        foreach (Entity entity in PlayerEntities)
        {
            if (entity.gameObject.GetComponent<AbstractNPCBrain>() != null 
                && entity.gameObject.GetComponent<AbstractNPCBrain>().currentState != null 
                && entity.gameObject.GetComponent<AbstractNPCBrain>().currentState.stateName != STATE.Idle) //is moving or attacking
            {
                can = false;
            }
        }

        return can;
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
            if (entity.gameObject.GetComponent<AbstractNPCBrain>().currentState.stateName == STATE.Move)
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
            if (entity.gameObject.GetComponent<AbstractNPCBrain>().currentState.stateName == STATE.Attack)
            {
                return entity;
            }
        }
        return null;
    }
    
    
}
