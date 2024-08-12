using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeathHandler : MonoBehaviour
{
    private GridManager gridManager;    
    private EnemyHealth enemyHealth;
    private Collider enemyCollider;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyCollider = GetComponent<Collider>();
        gridManager = FindObjectOfType<GridManager>();

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
        }
        else
        {
            Debug.LogError("Missing either collider component or grid manager reference.", this);
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= DeathHandler_OnDeath;
        }
    }
}
