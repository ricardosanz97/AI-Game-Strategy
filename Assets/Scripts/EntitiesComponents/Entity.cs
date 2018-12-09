using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour 
{
    public enum Owner
    {
        AI,
        Player
    }

    public Owner owner;
    public Rigidbody rb;

    public bool isTaskSuitable(AiTask aiTask)
    {
        throw new System.NotImplementedException();
    }

    public static System.Action OnTroopSpawned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
