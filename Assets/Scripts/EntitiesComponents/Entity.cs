using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour {
    public Rigidbody rb;
    public static System.Action OnTroopSpawned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
