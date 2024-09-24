using System;
using System.Collections;
using System.Collections.Generic;
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
            Logger.LogError("Enemy is missing Enemy script component.", this);
        } 

        maxHealth = enemy.HitPoints;
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other) 
    {
        Logger.LogWarning("Registering OnParticleCollision on Enemy unit", this);
        PlayerAbilities ability = other.GetComponentInParent<PlayerAbilities>();

        if (ability != null)
        {
            if (!hasCollided) // ensure vfx with multiple particles triggers TakeDamage only once and not once per particle.
            {
                Logger.Log($"{this.name} takes [{ability.Damage}] damage.", this);
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
            Logger.LogError("Missing PlayerAbilities.", this);
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
}
