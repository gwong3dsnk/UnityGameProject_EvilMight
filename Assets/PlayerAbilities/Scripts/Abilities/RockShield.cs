using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class RockShield : PlayerAbilities
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility(PlayerAbilities ability)
    {
        isEffectRepeating = false;
        base.ActivateAbility(ability);
    }

    protected override void SetParticleSystemLocationToSocket()
    {
        Logger.Log("Logic of SetParticleSystemLocationToSocket", this);
    }        

    protected override void ExecuteSecondaryActivationBehavior()
    {
        Logger.Log("Executing ExecuteSecondaryActivationBehavior");
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
