using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FingerFlick : AbilityBase
{
    #region Fields and Properties
    [SerializeField] private float meleeRange = 2.5f;
    private float activationDelay = 3.0f;
    private bool isAttacking = false;
    private bool isAvoidingAwakeActivation;
    private EnemyDeath enemyDeathHandler;
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

    #region Public Methods
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

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
    }    

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }        

    public override void HandleAnimEventFX()
    {
        Logger.Log($"[{this.name}] - 2. Set PARTICLE POSITION to ENEMY POSITION for [{this.name}] animation.", this);
        particleSystems[0].transform.position = enemyPosition;
        PlayParticleSystem(); 
    }    

    public override void UpgradeActivationDelay(float upgradeValue)
    {
        activationDelay = upgradeValue;
    }
    #endregion

    #region Protected Methods
    protected override void InitializeAbilityData()
    {
        base.InitializeAbilityData();
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

    private void GetAbilityParticleSystem()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        if (particleSystems.Length == 0) Logger.LogWarning("[FingerFlick] - No particle system gameobject components found.", this);
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

    private void PlayParticleSystem()
    {
        if (particleSystems[0].isPlaying)
        {
            Logger.Log($"[{this.name}] - 3. STOPPING particle system for [{this.name}].", this);
            particleSystems[0].Stop();
        }
        
        Logger.Log($"[{this.name}] - 3. STARTING particle system for [{this.name}].", this);
        particleSystems[0].Play();     
    }    
    #endregion
}
