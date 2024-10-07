using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class RockShield : AbilityBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility(AbilityBase ability)
    {
        isEffectRepeating = false;
    }  

    public override void HandlePlayAnimEventFX()
    {

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
