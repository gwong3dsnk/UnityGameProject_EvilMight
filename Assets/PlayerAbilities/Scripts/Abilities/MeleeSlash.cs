using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class MeleeSlash : PlayerAbilities
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility(PlayerAbilities ability)
    {
        Logger.Log("Activating MeleeSlash", this);
        isEffectRepeating = false;
        base.ActivateAbility(ability);
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
        Logger.Log("Executing ExecuteSecondaryActivationBehavior");
    }    

    protected override void InitializeAbilityData()
    {
        base.InitializeAbilityData();
    }
}
