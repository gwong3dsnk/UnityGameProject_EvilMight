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
        Debug.Log("Upgrading singleShot ability firerate");
        ParticleSystem singleShotFX = GetComponentInChildren<ParticleSystem>();
        var singleShotEmission = singleShotFX.emission;
        singleShotEmission.rateOverTime = 2;
    }
}
