using System.Collections;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FistSlam : AbilityBase
{
    private Coroutine attackCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    #region Public Methods
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Logger.Log($"Starting FistSlamAttackCoroutine for {this.name}");
        isAttacking = true;
        activationDelay = 10.0f;
        attackCoroutine = StartCoroutine(FistSlamAttackCoroutine());    
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        isAttacking = false;
        if (attackCoroutine != null) attackCoroutine = null;
    }    

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }    

    public override void HandleAnimEventFX()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        
        if (particleSystems.Length == 0) 
        {
            Logger.LogWarning("[FingerFlick] - No particle system gameobject components found.", this);
        }
        else
        {
            PlayParticleSystem();
        }
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
    private IEnumerator FistSlamAttackCoroutine()
    {
        while (isAttacking)
        {
            AbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 
            yield return new WaitForSeconds(activationDelay);
        }
    }

    private void PlayParticleSystem()
    {
        if (particleSystems[0].isPlaying) particleSystems[0].Stop();
        
        particleSystems[0].Play();
    }
    #endregion    
}
