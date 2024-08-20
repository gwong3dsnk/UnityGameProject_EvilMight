using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotProjectile : PlayerAbilities
{
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Debug.Log("Activating SingleShot", this);
    }

    public override void DeactivateAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void UpgradeAbility()
    {
        throw new System.NotImplementedException();
    }
}
