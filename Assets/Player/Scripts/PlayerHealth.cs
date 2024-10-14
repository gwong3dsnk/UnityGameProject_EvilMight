using System.Collections;
using UnityEngine;

public class PlayerHealth : HealthManagement
{
    [SerializeField] private int playerHealth = 3;
    private float deathDelay = 1.0f;
    private Enemy enemy;
    private bool isPlayerDead;

    protected override void Start()
    {
        maxHealth = playerHealth;
        base.Start();
    }

    private void OnParticleCollision(GameObject other) 
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

    protected override void BeginDeathSequence()
    {
        isPlayerDead = true;
        StartCoroutine(PlayerDeathCoroutine());
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        GetComponent<PlayerAnimController>().ProcessDeathAnim();
        yield return new WaitForSeconds(deathDelay);
        GetComponent<PlayerDeathHandler>().HandleDeath();
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

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
