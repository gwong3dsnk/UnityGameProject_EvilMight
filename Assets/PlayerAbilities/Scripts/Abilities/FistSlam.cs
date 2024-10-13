using System.Collections;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FistSlam : AbilityBase
{
    private float activationDelay = 10.0f;
    private ParticleSystem fxSystem;
    private Coroutine attackCoroutine;
    private bool isAttacking;

    protected override void Awake()
    {
        base.Awake();
    }

    #region Public Methods
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        StartUpFistSlamAttackCoroutine();
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

    public override void HandleAnimEventFX()
    {
        fxSystem = GetComponentInChildren<ParticleSystem>();
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

    private void PlayParticleSystem()
    {
        if (fxSystem.isPlaying)
        {
            fxSystem.Stop();
        }
        
        fxSystem.Play();
    }
    #endregion    
}
