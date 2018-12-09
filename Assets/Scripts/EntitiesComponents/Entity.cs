using System.Collections;
using System.Collections.Generic;
using AI.StrategicAI;
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
    public AiTask task;

    public static System.Action OnTroopSpawned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
}
