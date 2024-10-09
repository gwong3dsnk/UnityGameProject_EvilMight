using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : HealthManagement
{
    private Enemy enemy;

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

    public void ApplyAOEDamage(int damage)
    {
        TakeDamage(damage);
    }

    protected override void BeginPlayerDeathSequence()
    {
        TriggerDeathEvent();
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
