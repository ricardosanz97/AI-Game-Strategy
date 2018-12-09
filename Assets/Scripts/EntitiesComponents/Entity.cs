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
<<<<<<< HEAD
    public static System.Action OnTroopSpawned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
=======
>>>>>>> e0466d50877e2df8178a46d06db0605a34fec12a
}
