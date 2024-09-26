using System.Collections;
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
        Logger.LogWarning("[EnemyHealth] - Registering OnParticleCollision on Enemy unit", this);
        PlayerAbilities ability = other.GetComponentInParent<PlayerAbilities>();

        HandleTakeCollisionDamage(ability);
    }

    public void HandleTakeCollisionDamage(PlayerAbilities ability)
    {
        if (ability != null)
        {
            Logger.Log($"[EnemyHealth] - {this.name} takes [{ability.Damage}] damage.", this);
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

    protected override void HandleDeath()
    {
        TriggerDeathEvent();
    }

    protected override void HandleStillAlive()
    {
        // Enemy still lives, play GetHit anim state.
    }
}
