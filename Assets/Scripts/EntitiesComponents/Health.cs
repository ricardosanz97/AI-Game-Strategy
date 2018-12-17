using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Health : MonoBehaviour {

    public int health = 8;
    [HideInInspector]public int initialHealth;

    private void Start()
    {
        initialHealth = health;
    }

    public void ReceiveDamage(int amount)
    {
        health -= amount;
        
        if (GetComponent<Entity>().entityType != ENTITY.Core)
            GetComponent<AbstractNPCBrain>().sliderHealth.value = health;
        
        this.transform.DOShakePosition(0.3f, 0.2f);
        
        if (health <= 0)
            GetComponent<Entity>().Die();
    }

    public void SetHealth(int value)
    {
        health = value;
        GetComponent<AbstractNPCBrain>().sliderHealth.value = health;
    }
}
