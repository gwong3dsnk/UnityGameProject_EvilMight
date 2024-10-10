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
    #region Fields
    private float meleeRange = 5.0f;
    private bool isAttacking = false;
    private bool isAvoidingAwakeActivation;
    private float activationDelay = 3.0f;
    private EnemyDeathHandler enemyDeathHandler;
    private EnemyHealth enemyHealth;
    private Coroutine attackCoroutine;
    private Coroutine checkForEnemiesCoroutine;
    private ParticleSystem[] particleSystems;
    #endregion

    protected override void Awake()
    {
        isAvoidingAwakeActivation = false;
        base.Awake();
    }

    private void Update()
    {
        if (!isAttacking && checkForEnemiesCoroutine == null)
        {
            checkForEnemiesCoroutine = StartCoroutine(CheckIfEnemyInMeleeRangeCoroutine());
        }
    }

    #region IsEnemyInFront && IsEnemyInMeleeRange
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
        Dictionary<Collider, float> enemiesInMeleeRange = new Dictionary<Collider, float>();
        List<KeyValuePair<Collider, float>> sortedDictionary = new List<KeyValuePair<Collider, float>>();

        enemiesInMeleeRange = PopulateNearbyEnemyDictionary(enemiesInMeleeRange);

        if (enemiesInMeleeRange.Count == 0)
        {
            return false;
        }
        else
        {
            // Sort the dictionary so the closest enemy is the first element.
            sortedDictionary = enemiesInMeleeRange.OrderBy(enemy => enemy.Value).ToList();

            return ActIfEnemyInFront(sortedDictionary);
        }
    }

    private Dictionary<Collider, float> PopulateNearbyEnemyDictionary(Dictionary<Collider, float> enemiesInMeleeRange)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange);

        foreach (Collider collider in hitColliders)
        {
            // Isolate the Enemy objects and store them and their distance from this into the dictionary.
            if (collider.CompareTag("Enemy"))
            {
                enemyPosition = collider.transform.position;
                float enemyDistanceFromPlayer = Vector3.Distance(transform.position, enemyPosition);
                enemiesInMeleeRange.Add(collider, enemyDistanceFromPlayer);
            }
        }

        return enemiesInMeleeRange;
    }    

    private bool ActIfEnemyInFront(List<KeyValuePair<Collider, float>> sortedDictionary)
    {
        foreach (KeyValuePair<Collider, float> kvp in sortedDictionary)
        {
            Vector3 directionToEnemy = (kvp.Key.transform.position - transform.position).normalized;

            if (Vector3.Dot(transform.forward, directionToEnemy) > 0.7f)
            {
                Logger.Log($"[FingerFlick] - {kvp.Key} is {kvp.Value}m in front of player.", this);
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
    #endregion

    private void GetAbilityParticleSystem()
    {
        // Retrieve the runtime gameobject's particle system component.
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        if (particleSystems.Length == 0)
        {
            Logger.LogWarning("[FingerFlick] - No particle system gameobject components found.", this);
        }
    }        
    
    private void SubscribeToEnemyDeathHandlerEvent(Collider collider)
    {
        enemyDeathHandler = collider.GetComponent<EnemyDeathHandler>();
        enemyDeathHandler.OnEnemyDeactivation += StopAttacking;
        enemyHealth = collider.GetComponent<EnemyHealth>(); // Setup enemyHealth reference.
    }

    #region Handle ability attack coroutine
    public override void ActivateAbility()
    {
        // isAvoidingAwakeActivation is necessary to avoid starting of AttackCoroutine during Awake stage.
        if (isAvoidingAwakeActivation)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                GetAbilityParticleSystem();
                Logger.Log("[FingerFlick] - Starting AttackCoroutine", this);
                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
        else
        {
            isAvoidingAwakeActivation = true; // After Awake stage, set to true for remainder of game.
        }
    }    

    private IEnumerator AttackCoroutine()
    {
        while(isAttacking)
        {
            Logger.Log($"[FingerFlick] - Playing through AttackCoroutine...playing anim and waiting {activationDelay} seconds.", this);
            AbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 
            enemyHealth.HandleTakeCollisionDamage(this);
            yield return new WaitForSeconds(activationDelay);
            
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
            Logger.Log("[FingerFlick] - Stopping attack cycle for FingerFlick anim and coroutine", this);
            isAttacking = false;
            DeactivateAbility();
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            enemyDeathHandler.OnEnemyDeactivation -= StopAttacking;
        }
    }    
    #endregion

    public override void HandlePlayAnimEventFX()
    {
        Logger.Log($"[PlayerAbilitiesManager] - Setting {particleSystems[0].name} position to {enemyPosition}", this);
        particleSystems[0].transform.position = enemyPosition;
        PlayParticleSystem();
    }

    private void PlayParticleSystem()
    {
        if (particleSystems[0].isPlaying)
        {
            Logger.Log("Stopping particle system.", this);
            particleSystems[0].Stop();
        }
        
        Logger.Log("Starting particle system.", this);
        particleSystems[0].Play();     
    }    

    public override void UpgradeActivationDelay(float upgradeValue)
    {
        activationDelay = upgradeValue;
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
    }

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }

    protected override void InitializeAbilityData()
    {
        base.InitializeAbilityData();
    }
}
