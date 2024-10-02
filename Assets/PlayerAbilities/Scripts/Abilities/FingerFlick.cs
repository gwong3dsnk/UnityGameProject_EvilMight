using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FingerFlick : PlayerAbilities
{
    private float meleeRange = 5.0f;
    private bool isAttacking = false;
    private bool isAvoidingAwakeActivation;
    private EnemyDeathHandler enemyDeathHandler;
    private EnemyHealth enemyHealth;
    private Coroutine attackCoroutine;
    private Coroutine checkForEnemiesCoroutine;

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

    private IEnumerator CheckIfEnemyInMeleeRangeCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (CheckIfEnemyInFront())
        {
            ActivateAbility(this);
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

    private void SubscribeToEnemyDeathHandlerEvent(Collider collider)
    {
        enemyDeathHandler = collider.GetComponent<EnemyDeathHandler>();
        enemyDeathHandler.OnEnemyDeactivation += StopAttacking;
        enemyHealth = collider.GetComponent<EnemyHealth>(); // Setup enemyHealth reference.
    }

    public override void ActivateAbility(PlayerAbilities ability)
    {
        // isAvoidingAwakeActivation is necessary to avoid starting of AttackCoroutine during Awake stage.
        if (isAvoidingAwakeActivation)
        {
            if (!isAttacking)
            {
                activationDelay = 3.0f;
                isAttacking = true;
                base.ActivateAbility(ability);
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
            PlayerAbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(); 
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

    protected override void SetParticleSystemLocationToSocket()
    {
        Logger.Log("FingerFlick not using SetParticleSystemLocationToSocket", this); 
    }        

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
    }

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }

    protected override void ExecuteSecondaryActivationBehavior()
    {
        Logger.Log("Fingerflick not using ExecuteSecondaryActivationBehavior", this);
    }    

    protected override void InitializeAbilityData()
    {
        base.InitializeAbilityData();
    }
}
