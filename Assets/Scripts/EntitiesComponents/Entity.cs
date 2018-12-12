using System.Collections;
using System.Collections.Generic;
using StrategicAI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

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
}
