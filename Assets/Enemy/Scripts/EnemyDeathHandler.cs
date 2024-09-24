using System;
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
    public event EventHandler OnEnemyDeactivation;

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
            Logger.LogError("[EnemyDeathHandler] - Unable to find GridManagerInstance", this);
        }

        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += DeathHandler_OnDeath;
        }
        else
        {
            Logger.LogError("[EnemyDeathHandler] - Missing EnemyHealth script component.", this);
        }    
    }

    private void DeathHandler_OnDeath(object sender, System.EventArgs e)
    {
        if (enemyCollider != null && gridManager != null)
        {
            Logger.Log("[EnemyDeathHandler] - Processing enemy death.", this);
            enemyHealth.OnDeath -= DeathHandler_OnDeath;
            gridManager.RemoveEnemy(enemyCollider);
            gameObject.SetActive(false);
            PassXPToPlayer();
            OnEnemyDeactivation?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Logger.LogError("[EnemyDeathHandler] - Missing either collider component or grid manager reference.", this);
        }
    }

    private void PassXPToPlayer()
    {
        if (enemy != null && levelManager != null)
        {
            levelManager.AddXP(enemy.Experience);
        }
        else
        {
            Logger.LogError("[EnemyDeathHandler] - Missing reference to Enemy and/or LevelManager", this);
        }
    }
}
