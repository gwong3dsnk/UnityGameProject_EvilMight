using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeSlash : PlayerAbilities
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Debug.Log("Activating MeleeSlash", this);
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
