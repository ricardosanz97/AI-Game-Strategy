using System.Collections;
using System.Collections.Generic;
using StrategicAI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
public enum TryingTo
{
    Move,
    Attack,
    None
}
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Entity : MonoBehaviour 
{
    public int bloodCost;
    
    [HideInInspector]public CellBehaviour cell;
    public enum Owner
    {
        AI,
        Player
    }

    [HideInInspector]public Owner owner;
    [HideInInspector]public Rigidbody rb;
    public float value;
    public TryingTo tryingTo = TryingTo.None;
    [HideInInspector] [Inject] public SpawnablesManager _spawnablesManager;
    [HideInInspector]public LevelController _levelController;

    [HideInInspector]public static System.Action<Entity> OnTroopDeleted;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _levelController = FindObjectOfType<LevelController>();
    }

    public void Die()
    {
        OnTroopDeleted?.Invoke(this);
    }

    public void SetEntity(Owner owner) {
        this.owner = owner;
        if (owner == Owner.Player)
        {
            _levelController.PlayerEntities.Add(this);
        }
        else if (owner == Owner.AI)
        {
            _levelController.AIEntities.Add(this);
        }
    }
}
