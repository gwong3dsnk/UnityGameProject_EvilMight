using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyDeathHandler : MonoBehaviour
{
    private Enemy enemy;
    private EnemyHealth enemyHealth;
    private Collider enemyCollider;
    private LevelManager levelManager;
    private Animator animator;
    private float deactivationDelay = 1.0f;
    public event EventHandler OnEnemyDeactivation;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyCollider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();
        levelManager = FindObjectOfType<LevelManager>();
        
        if (GridManager.GridManagerInstance == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Unable to find GridManagerInstance", this);
            return;
        }

        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += DeathHandler_OnDeath;
        }
        else
        {
            Logger.LogError("[EnemyDeathHandler] - Missing EnemyHealth script component.", this);
            return;
        }    

        if (enemy == null || enemyCollider == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Missing either Enemy or Collider components.", this);
            return;
        }

        if (levelManager == null || animator == null)
        {
            Logger.LogError("[EnemyDeathHandler] - Missing reference to either LevelManager or Animator components.", this);
            return;
        }
    }

    private void DeathHandler_OnDeath(object sender, System.EventArgs e)
    {
        Logger.Log("[EnemyDeathHandler] - Play enemy death anim, wait, then process enemy deactivation.", this);
        animator.SetTrigger("DeathTrigger");
        OnEnemyDeactivation?.Invoke(this, EventArgs.Empty);
        enemyHealth.OnDeath -= DeathHandler_OnDeath;
        levelManager.AddXP(enemy.Experience);
                
        StartCoroutine(DelayProcessingEnemyDeactivation());
        Logger.Log("[EnemyDeathHandler] - Finished processing enemy deactivation.", this);
    }

    private IEnumerator DelayProcessingEnemyDeactivation()
    {
        yield return new WaitForSeconds(deactivationDelay);

        GridManager.GridManagerInstance.RemoveEnemy(enemyCollider);
        gameObject.SetActive(false);
    }
}
