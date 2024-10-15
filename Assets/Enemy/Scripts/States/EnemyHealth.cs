using UnityEngine;

// [RequireComponent(typeof(Enemy))]
public class EnemyHealth : HealthManagement
{
    private Enemy enemy;
    private bool isEnemyDeathProcessed = false;

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Logger.LogError("[EnemyHealth] - Enemy is missing Enemy script component.", this);
        } 

        maxHealth = enemy.HitPoints;
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other)
    {
        AbilityBase ability = other.GetComponentInParent<AbilityBase>();
        HandleTakeCollisionDamage(ability);
    }

    public void HandleTakeCollisionDamage(AbilityBase ability)
    {
        if (ability != null)
        {
            TakeDamage(ability.Damage);
        }
        else
        {
            Logger.LogError("[EnemyHealth] - Missing PlayerAbilities.", this);
        }
    }

    protected override void BeginDeathSequence()
    {
        if (!isEnemyDeathProcessed)
        {
            InvokeOnDeathEvent();
            isEnemyDeathProcessed = true;
        }
    }

    protected override void HandleStillAlive()
    {
        // Enemy still lives, play GetHit anim state.
        EnemyAnimController enemyAnimController = GetComponentInChildren<EnemyAnimController>();
        if (enemyAnimController != null)
        {
            enemyAnimController.DetermineEnemyClassAndAction(EnemyAnimCategory.GetHit);
        }
        else
        {
            Logger.LogError($"Missing reference to EnemyAnimController in {this.name}", this);
        }
    }
}
