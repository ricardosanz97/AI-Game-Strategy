using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using InfluenceMap;
using StrategicAI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Node = CustomPathfinding.Node;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Entity : MonoBehaviour 
{
    public int bloodCost;
    public ENTITY entityType = ENTITY.None;

    public CellBehaviour cell;
    public enum Owner
    {
        AI,
        Player
    }

    public Owner owner;
    [HideInInspector]public Rigidbody rb;
    public float value;
    [HideInInspector] [Inject] public SpawnablesManager _spawnablesManager;
    [HideInInspector]public LevelController _levelController;
    [HideInInspector]public static System.Action<Entity> OnTroopDeleted;
    public InfluenceMap.InfluenceMapComponent _influenceMapComp; 

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _levelController = FindObjectOfType<LevelController>();
        _influenceMapComp = FindObjectOfType<InfluenceMap.InfluenceMapComponent>();
    }

    [ContextMenu("Provoke Death")]
    public void Die()
    {
        this.transform.DOLocalMoveY(-100f, 59f);
        
        //todo hacer que el pathfinding y el influence
        
        Destroy(this.gameObject,1f);
        OnTroopDeleted?.Invoke(this);

    }

    public void SetEntity(Owner owner) {

        this.owner = owner;

        if (owner == Owner.Player)
        {
            _levelController.AddPlayerEntities(this);
        }
        else if (owner == Owner.AI)
        {
            _levelController.AddAIEntities(this);
        }
    }
}
