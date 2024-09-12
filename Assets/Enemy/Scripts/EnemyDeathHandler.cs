using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeathHandler : MonoBehaviour
{
    private Enemy enemy;
    private EnemyHealth enemyHealth;
    private Collider enemyCollider;
    private GridManager gridManager;
    private LevelManager levelManager;
    // public event Action<int> OnEnemyDeath_PassPlayerXP;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyCollider = GetComponent<Collider>();
        levelManager = FindObjectOfType<LevelManager>();
        
        if (GridManager.GridManagerInstance != null)
        {
            gridManager = GridManager.GridManagerInstance;
        }
        else
        {
            Logger.LogError("Unable to find GridManagerInstance", this);
        }

        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += DeathHandler_OnDeath;
        }
        else
        {
            Logger.LogError("Missing EnemyHealth script component.", this);
        }    
    }

    // private void OnDisable() 
    // {
    //     enemyHealth.OnDeath -= DeathHandler_OnDeath;
    // }

    private void DeathHandler_OnDeath(object sender, System.EventArgs e)
    {
        if (enemyCollider != null && gridManager != null)
        {
            gridManager.RemoveEnemy(enemyCollider);
            gameObject.SetActive(false);
            PassXPToPlayer();
        }
        else
        {
            Logger.LogError("Missing either collider component or grid manager reference.", this);
        }
    }

    private void PassXPToPlayer()
    {
        if (enemy != null && levelManager != null)
        {
            // OnEnemyDeath_PassPlayerXP?.Invoke(enemy.Experience);
            levelManager.AddXP(enemy.Experience);
        }
        else
        {
            Logger.LogError("Missing reference to Enemy and/or LevelManager", this);
        }
    }
}
