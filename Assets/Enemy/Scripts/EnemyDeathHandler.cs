using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeathHandler : MonoBehaviour
{
    public event EventHandler OnEnemyDeactivation; 
    private float deactivationDelay = 1.5f;
    private Enemy enemy;
    private EnemyHealth enemyHealth;
    private Collider enemyCollider;
    private LevelManager levelManager;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Missing EnemyHealth script component.", this);
            return;
        }
    }

    private void OnEnable()
    {
        enemyHealth.OnDeath += DeathHandler_OnDeath;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyCollider = GetComponent<Collider>();
        levelManager = FindObjectOfType<LevelManager>();
        
        if (GridManager.GridManagerInstance == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Unable to find GridManagerInstance", this);
            return;
        }

        if (enemy == null || enemyCollider == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Missing either Enemy or Collider components.", this);
            return;
        }

        if (levelManager == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Missing reference to LevelManager.", this);
            return;
        }
    }

    private void OnDisable()
    {
        enemyHealth.OnDeath -= DeathHandler_OnDeath;
    }


    private void DeathHandler_OnDeath(object sender, System.EventArgs e)
    {
        EnemyHealth enemyHealthSender = sender as EnemyHealth;

        if (enemyHealthSender != null)
        {
            Logger.Log("[EnemyDeathHandler] - Pass enemy xp to level manager, then process enemy deactivation.", this);
            levelManager.AddXP(enemy.Experience);

            StartCoroutine(DelayProcessingEnemyDeactivation());
            Logger.Log("[EnemyDeathHandler] - Finished processing enemy deactivation.", this);
        }        
    }

    private IEnumerator DelayProcessingEnemyDeactivation()
    {
        yield return new WaitForSeconds(deactivationDelay);

        GridManager.GridManagerInstance.RemoveEnemy(enemyCollider);
        gameObject.SetActive(false);
        OnEnemyDeactivation?.Invoke(this, EventArgs.Empty);
    }
}
