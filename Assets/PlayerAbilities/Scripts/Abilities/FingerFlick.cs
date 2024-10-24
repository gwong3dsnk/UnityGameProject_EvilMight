using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FingerFlick : AbilityBase
{
    #region Fields and Properties
    private bool isAvoidingAwakeActivation;
    private EnemyDeath enemyDeathHandler;
    private EnemyHealth enemyHealth;
    private Coroutine attackCoroutine;
    private Coroutine checkForEnemiesCoroutine;
    #endregion

    protected override void Awake()
    {
        isAvoidingAwakeActivation = false;
        base.Awake();
        gameObject.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();

        if (!isAttacking && checkForEnemiesCoroutine == null)
        {
            checkForEnemiesCoroutine = StartCoroutine(CheckIfEnemyInMeleeRangeCoroutine());
        }
    }

    #region Public Methods
    public override void ActivateAbility()
    {
        particleSystems[0].gameObject.SetActive(true);

        // isAvoidingAwakeActivation is necessary to avoid starting of AttackCoroutine during Awake stage.
        if (isAvoidingAwakeActivation)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                if (particleSystems.Length == 0)
                {
                    Logger.LogError("Missing vfx reference for FingerFlick ability.", this);
                }

                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
        else
        {
            isAvoidingAwakeActivation = true; // After Awake stage, set to true for remainder of game.
        }
    }    

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        isAttacking = true; // prevent update logic from executing.
    }    

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }        

    public override void HandleAnimEventFX()
    {
        particleSystems[0].transform.position = enemyPosition;
        PlayParticleSystem(); 
    }    
    #endregion

    #region Protected Methods
    protected override void UpgradeAttackSpeed(float upgradeValue)
    {
        attackSpeed = upgradeValue;
    }
    #endregion

    #region Private Methods
    private IEnumerator CheckIfEnemyInMeleeRangeCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (CheckIfEnemyInFront())
        {
            ActivateAbility();
        }
        else
        {
            StopAttacking(null, EventArgs.Empty);
        }

        checkForEnemiesCoroutine = null;
    }

    private bool CheckIfEnemyInFront()
    {
        List<KeyValuePair<Collider, float>> sortedDictionary = new List<KeyValuePair<Collider, float>>();
        Dictionary<Collider, float> enemiesInRange = base.GetNearbyEnemies(transform.position, abilityRange);

        if (enemiesInRange.Count == 0)
        {
            return false;
        }
        else
        {
            // Sort the dictionary so the closest enemy is the first element.
            sortedDictionary = enemiesInRange.OrderBy(enemy => enemy.Value).ToList();
            return ActIfEnemyInFront(sortedDictionary);
        }
    }

    private bool ActIfEnemyInFront(List<KeyValuePair<Collider, float>> sortedDictionary)
    {
        foreach (KeyValuePair<Collider, float> kvp in sortedDictionary)
        {
            Vector3 directionToEnemy = (kvp.Key.transform.position - transform.position).normalized;

            if (Vector3.Dot(transform.forward, directionToEnemy) > 0.7f)
            {
                SubscribeToEnemyDeathHandlerEvent(kvp.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }   
    
    private void SubscribeToEnemyDeathHandlerEvent(Collider collider)
    {
        enemyDeathHandler = collider.GetComponent<EnemyDeath>();
        enemyDeathHandler.OnEnemyDeactivation += StopAttacking;
        enemyHealth = collider.GetComponent<EnemyHealth>(); // Setup enemyHealth reference.
    }

    private IEnumerator AttackCoroutine()
    {
        while(isAttacking)
        {
            AbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 
            enemyHealth.TakeGeneralDamage(this);
            yield return new WaitForSeconds(attackSpeed);
            
            if (!CheckIfEnemyInFront())
            {
                StopAttacking(null, EventArgs.Empty);
                yield break;
            }
        }
    }

    private void StopAttacking(object sender, System.EventArgs e)
    {
        // Call this method from enemy when the enemy prefab dies and is disabled.
        if (isAttacking)
        {
            isAttacking = false;
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            enemyDeathHandler.OnEnemyDeactivation -= StopAttacking;
        }
    }   

    private void PlayParticleSystem()
    {
        if (particleSystems[0].isPlaying)
        {
            particleSystems[0].Stop();
        }
        
        particleSystems[0].Play();     
    }    
    #endregion
}
