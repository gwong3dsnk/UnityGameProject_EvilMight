using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : HealthManagement
{
    private Enemy enemy;
    private PlayerAbilities ability;

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError("Enemy is missing Enemy script component.", this);
        } 

        maxHealth = enemy.HitPoints;
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other) 
    {
        Debug.Log(other.transform.name);
        ability = other.GetComponentInParent<PlayerAbilities>();
        if (ability != null)
        {
            TakeDamage(ability.Damage);
        }
        else
        {
            Debug.LogError("Missing PlayerAbilities.", this);
        }
    }

    protected override void HandleDeath()
    {
        gameObject.SetActive(false);
        TriggerDeathEvent();
    }
    
}