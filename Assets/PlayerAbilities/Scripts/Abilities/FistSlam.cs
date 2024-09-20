using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FistSlam : PlayerAbilities
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility(PlayerAbilities ability)
    {
        activationDelay = 8.0f;
        isEffectRepeating = true;
        base.ActivateAbility(ability);
    }

    protected override void SetParticleSystemLocationToSocket()
    {
        Logger.Log("Logic of SetParticleSystemLocationToSocket", this);
    }        

    protected override void ExecuteSecondaryActivationBehavior()
    {
        Logger.Log($"{this.name} has no SecondaryActivationBehavior logic to run.", this);
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
