﻿using System.Collections;
using System.Collections.Generic;
using AI.StrategicAI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour 
{
    public int bloodCost;
    public enum Owner
    {
        AI,
        Player
    }

    public Owner owner;
    public Rigidbody rb;
    public AiTask task;
    public float value;


    public LevelController _levelController;

    public static System.Action<Entity> OnTroopDeleted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _levelController = FindObjectOfType<LevelController>().GetComponent<LevelController>();
    }

    public void Assign(AIResourcesAllocator.PossibleTaskAssignment possibleTaskAssignment)
    {
        //if task is assigned return
        if (task != null) return;

        task = possibleTaskAssignment.Task;
        possibleTaskAssignment.Task.Assign(this);
    }
    
    public bool isTaskSuitable(AiTask aiTask)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        OnTroopDeleted?.Invoke(this);
    }
}
