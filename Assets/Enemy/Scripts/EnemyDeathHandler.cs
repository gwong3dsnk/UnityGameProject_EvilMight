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
            Debug.LogError("Unable to find GridManagerInstance", this);
        }

        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += DeathHandler_OnDeath;
        }
        else
        {
            Debug.LogError("Missing EnemyHealth script component.", this);
        }    
    }

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
            Debug.LogError("Missing either collider component or grid manager reference.", this);
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
            Debug.LogError("Missing reference to Enemy and/or LevelManager", this);
        }
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= DeathHandler_OnDeath;
        }
    }
}
