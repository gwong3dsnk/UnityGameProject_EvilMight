using System;
using System.Collections;
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
    private Coroutine attackCoroutine;

    protected override void Awake()
    {
        isAvoidingAwakeActivation = false;
        base.Awake();
    }

    private void Update()
    {
        if (!isAttacking)
        {
            CheckIfEnemyInMeleeRange();
        }
    }
    
    private void CheckIfEnemyInMeleeRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Logger.Log($"Nearby Enemy collider detected - {collider.gameObject.name} witihn {meleeRange}m");
                enemyPosition = collider.transform.position;
                Vector3 directionToEnemy = (enemyPosition - transform.position).normalized;
                
                if (Vector3.Dot(transform.forward, directionToEnemy) > 0.5f)
                {
                    Logger.Log("Enemy in front of player, calling ActivateAbility.", this);
                    ActivateAbility(this);
                    return;
                } 
                else
                {
                    // Enemy not in front of player, StopCoroutine if it's running.
                    isAttacking = false;
                    if (attackCoroutine != null)
                    {
                        StopCoroutine(attackCoroutine);
                    }
                }
            }
        }
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
                Logger.Log("Starting AttackCoroutine", this);
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
            // Logic to set trigger to play animation, making sure to play it only once.
            Logger.Log($"Playing through AttackCoroutine...playing anim and waiting {activationDelay} seconds.", this);
            PlayerAbilitiesManager.AbilityManagerInstance.InvokeOnActivationCompletion();
            yield return new WaitForSeconds(activationDelay);
        }
    }

    private void StopAttacking() // Call this method from enemy when the enemy prefab dies and is disabled.
    {
        if (isAttacking)
        {
            Logger.Log("Stopping attack cycle for FingerFlick anim and coroutine", this);
            isAttacking = false;
            DeactivateAbility();
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
