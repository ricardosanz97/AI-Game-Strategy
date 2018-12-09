﻿using System.Collections;
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
}
