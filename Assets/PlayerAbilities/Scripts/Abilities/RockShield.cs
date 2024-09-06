using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockShield : PlayerAbilities
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Logger.Log("Activating RockShield", this);
    }
    public override void DeactivateAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateUpgrade(Dictionary<string, AbilityUpgrades> upgrade)
    {
        base.ActivateUpgrade(upgrade);
    }

    protected override void InitializeAbilityData()
    {
        base.InitializeAbilityData();
    }
}
