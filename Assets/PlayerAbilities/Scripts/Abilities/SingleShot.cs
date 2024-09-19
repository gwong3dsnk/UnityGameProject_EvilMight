using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class SingleShot : PlayerAbilities
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
        Logger.Log("Starting to set particle system location to socket indexSocket.L.", this);
        string indexSocketLeft = "indexSocket.L";
        // string indexSocketRight = "indexSocket.R";

        GameObject indexLeftSocket = playerSockets.FirstOrDefault(socket => socket.name == indexSocketLeft);
        Vector3 indexLeftSocketPosition = indexLeftSocket.transform.position;
        Logger.Log($"indexSocket.L position - {indexLeftSocketPosition}");
        transform.position = indexLeftSocketPosition;
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
