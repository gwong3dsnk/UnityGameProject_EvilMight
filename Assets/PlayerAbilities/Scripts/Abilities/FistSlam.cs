using System;
using System.Collections;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FistSlam : AbilityBase
{
    private ParticleSystem fxSystem;
    private Coroutine attackCoroutine;
    private bool isAttacking;
    private float activationDelay = 10.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        StartUpFistSlamAttackCoroutine();
    }

    private void StartUpFistSlamAttackCoroutine()
    {
        Logger.Log($"Starting FistSlamAttackCoroutine for {this.name}");
        isAttacking = true;
        attackCoroutine = StartCoroutine(FistSlamAttackCoroutine());        
    }

    private IEnumerator FistSlamAttackCoroutine()
    {
        while (isAttacking)
        {
            AbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 
            yield return new WaitForSeconds(activationDelay);
        }
    }

    private void StopFistSlamAttackCoroutine()
    {
        if (isAttacking)
        {
            isAttacking = false;
            StopCoroutine(attackCoroutine);
            if (attackCoroutine != null)
            {
                attackCoroutine = null;
            }
        }
    }

    public override void HandlePlayAnimEventFX()
    {
        fxSystem = GetComponentInChildren<ParticleSystem>();
        PlayParticleSystem();
    }

    private void PlayParticleSystem()
    {
        if (fxSystem.isPlaying)
        {
            fxSystem.Stop();
        }
        
        fxSystem.Play();
    }

    public override void UpgradeActivationDelay(float upgradeValue)
    {
        activationDelay = upgradeValue;
    }    

    public override void DeactivateAbility()
    {
        StopFistSlamAttackCoroutine();
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
