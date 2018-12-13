﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Health : MonoBehaviour {

    public int health = 8;
    private int initialHealth;

    private void Start()
    {
        initialHealth = health;
    }

    public void ReceiveDamage(int amount)
    {
        health -= amount;
        GetComponent<AbstractNPCBrain>().sliderHealth.value = health;
        this.transform.DOShakePosition(0.3f, 0.2f);
    }
}
