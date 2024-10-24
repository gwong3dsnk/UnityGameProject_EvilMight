using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeath : MonoBehaviour
{
    public event EventHandler OnEnemyDeactivation; 
    private float deactivationDelay = 1.5f;
    private float colliderDisableDelay = 0.5f;
    private Enemy enemy;
    private EnemyHealth enemyHealth;
    private Collider enemyCollider;
    private PlayerLevelingManager playerLevelingManager;
    private EnemyPool enemyPool;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyPool = FindObjectOfType<EnemyPool>();

        if (enemyHealth == null || enemyPool == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Missing EnemyHealth component or reference to EnemyPool.", this);
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
        playerLevelingManager = FindObjectOfType<PlayerLevelingManager>();
        
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

        if (playerLevelingManager == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Missing reference to PlayerLevelingManager.", this);
            return;
        }
    }

    private void OnDisable()
    {
        enemyHealth.OnDeath -= DeathHandler_OnDeath;
    }


    private void DeathHandler_OnDeath(object sender, System.EventArgs e)
    {
        if (sender as EnemyHealth != null)
        {
            bool isGameOver = enemyPool.DecrementFinalWaveCounter();
            if (!isGameOver)
            {
                playerLevelingManager.AddXP(enemy.Experience);
                StartCoroutine(DelayProcessingEnemyDeactivation());
            }
        }        
    }

    private IEnumerator DelayProcessingEnemyDeactivation()
    {
        yield return new WaitForSeconds(colliderDisableDelay);
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(deactivationDelay);
        GridManager.GridManagerInstance.RemoveEnemy(enemyCollider);
        // EnqueueKilledEnemy(); // Enable if you want to have the waves continue to loop indefinitely.  Will need to fix issue of enemies not moving after reactivating.
        OnEnemyDeactivation?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }

    private void EnqueueKilledEnemy()
    {
        string parentName = transform.parent.gameObject.name;
        enemyPool.WaveEnemies[parentName].Enqueue(gameObject);
    }    
}
