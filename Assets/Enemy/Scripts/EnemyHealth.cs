using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : HealthManagement
{
    private Enemy enemy;
    private bool hasCollided = false;

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

    private void HandleTakeCollisionDamage(PlayerAbilities ability)
    {
        if (ability != null)
        {
            if (!hasCollided) // ensure vfx with multiple particles triggers TakeDamage only once and not once per particle.
            {
                Logger.Log($"[EnemyHealth] - {this.name} takes [{ability.Damage}] damage.", this);
                TakeDamage(ability.Damage);

                if (this.gameObject.activeInHierarchy == true) // confirm the enemy prefab gameobject is active before attempting coroutine
                {
                    if (ability.AbilityName == AbilityNames.FingerFlick)
                    {
                        hasCollided = true;
                        StartCoroutine(ResetCollisionFlag(1.0f));
                    }
                }
            }
        }
        else
        {
            Logger.LogError("[EnemyHealth] - Missing PlayerAbilities.", this);
        }
    }

    private IEnumerator ResetCollisionFlag(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hasCollided = false;
    }

    public void ApplyAOEDamage(int damage)
    {
        TakeDamage(damage);
    }

    // public void ApplyAOEDamage(int damage, GameObject[] killedEnemies)
    // {
    //     foreach (GameObject enemy in killedEnemies)
    //     {
    //         TakeDamage(damage);
    //     }
    // }

    protected override void HandleDeath()
    {
        TriggerDeathEvent();
    }

    protected override void HandleStillAlive()
    {
        // Enemy still lives, play GetHit anim state.
    }
}
