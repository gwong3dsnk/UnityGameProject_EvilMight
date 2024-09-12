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
            Logger.LogError("Enemy is missing Enemy script component.", this);
        } 

        maxHealth = enemy.HitPoints;
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other) 
    {
        ability = other.GetComponentInParent<PlayerAbilities>();
        if (ability != null)
        {
            TakeDamage(ability.Damage);
        }
        else
        {
            Logger.LogError("Missing PlayerAbilities.", this);
        }
    }

    public void ApplyAOEDamage(int damage)
    {
        TakeDamage(damage);
    }

    // public void ApplyAOEDamage(int damage, GameObject[] killedEnemies)
    // {
    //     foreach (GameObject enemy in killedEnemies)
    //     {
    //         TakeDamage(damage);
    //     }
    // }

    protected override void HandleDeath()
    {
        TriggerDeathEvent();
    }
}
