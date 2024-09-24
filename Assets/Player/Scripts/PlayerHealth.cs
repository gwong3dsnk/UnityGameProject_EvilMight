using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthManagement
{
    [SerializeField] int playerHealth = 3;
    private Enemy enemy;

    protected override void Start()
    {
        maxHealth = playerHealth;
        base.Start();
    }

    private void OnParticleCollision(GameObject other) 
    {
        Logger.Log("Detecting particle collision hit on player", this);
        GameObject enemyPrefab = other.transform.parent?.gameObject;
        enemy = enemyPrefab.GetComponent<Enemy>();

        if (enemy != null)
        {
            TakeDamage(enemy.Attack);
        }
    }

    protected override void HandleDeath()
    {
        GetComponent<PlayerDeathHandler>().HandleDeath();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
