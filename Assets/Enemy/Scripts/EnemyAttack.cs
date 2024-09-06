using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] ParticleSystem attackFX;

    public void AttackTarget(Enemy enemy)
    {
        if (enemy.EnemyClass == EnemyClass.Range && enemy.EnemyDifficulty == EnemyDifficulty.Easy)
        {
            if (attackFX == null)
            {
                Logger.LogError("Missing enemy projectile FX reference.");
            }
            else
            {
                var projectileEmission = attackFX.emission;
                projectileEmission.enabled = true;
            }
        }
    }
}
