using System.Collections;
using UnityEngine;

public class PlayerHealth : HealthManagement
{
    [SerializeField] private int playerHealth = 3;
    private Enemy enemy;
    private bool isPlayerDead;

    protected override void Start()
    {
        maxHealth = playerHealth;
        base.Start();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }    

    public void TakePhysicalDamage(int damage)
    {
        TakeDamage(damage);
    }

    protected override void OnParticleCollision(GameObject other) 
    {
        if (!isPlayerDead)
        {
            GameObject enemyPrefab = other.transform.parent?.gameObject;
            enemy = enemyPrefab.GetComponent<Enemy>();

            if (enemy != null)
            {
                TakeDamage(enemy.Attack);
            }
        }
    } 

    protected override void HandleDeath()
    {
        isPlayerDead = true;
        InvokeOnDeathEvent();
    }

    protected override void HandleStillAlive()
    {
        isPlayerDead = false;
        // GetComponent<PlayerAnimController>().ProcessGetHitAnim();
        // TODO: The logic  below should make sure that the GetHit animation does't take priority away from other animation states.

        // AnimatorStateInfo stateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        // bool isAnimPlaying = false;

        // if (stateInfo.IsName("GetHit") && stateInfo.normalizedTime < 1.0f)
        // {
        //     Logger.Log($"Anim is still playing - GetHit");
        //     isAnimPlaying = true;
        // }
        // else
        // {
        //     var abilityNamesValues = System.Enum.GetValues(typeof(AbilityNames));
        //     foreach (AbilityNames name in abilityNamesValues)
        //     {
        //         if (stateInfo.IsName(name.ToString()) && stateInfo.normalizedTime < 1.0f)
        //         {
        //             Logger.Log($"Anim is still playing - {name}");
        //             isAnimPlaying = true;
        //             break;
        //         }
        //         else
        //         {
        //             Logger.Log($"No anim is playing - {name}");
        //             isAnimPlaying = false;
        //         }
        //     }
        // }

        // if (!isAnimPlaying)
        // {
        //     Logger.Log($"Playing GetHit anim.");
        //     GetComponent<PlayerAnimController>().ProcessGetHitAnim();
        // }
    }
}
