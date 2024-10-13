using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public abstract class AbilityBase : MonoBehaviour
{
    #region Fields and Properties
    // SerializedFields
    [SerializeField] protected AbilityLibraryData abilityData;
    [SerializeField] protected AbilityNames abilityName;  
    [SerializeField] protected float damage;   
    [SerializeField] protected float fireRate;
    [SerializeField] protected float attackSpeed;

    // Public Fields/Properties
    public AbilityNames AbilityName => abilityName;
    public float Damage { get => damage; set => damage = value; }
    public float FireRate => fireRate;

    // Protected Fields
    protected bool isEffectRepeating = false;
    protected GameObject[] playerSockets;
    protected Vector3 enemyPosition = new Vector3();      
    #endregion

    protected virtual void Awake()
    {
        InitializeAbilityData();
    }

    #region Public Abstract Methods
    public abstract void HandleAnimEventFX();

    public abstract void UpgradeActivationDelay(float upgradeValue);
    #endregion

    #region Public Virtual Methods
    public virtual void ActivateAbility()
    {
        UpgradeManager.UpgradeManagerInstance.UpgradeDatabaseManager.CheckIfUpgradesInDatabase();
    }

    public virtual void DeactivateAbility()
    {
        Logger.Log($"[{this.name}] - Stopping particle system.", this);
    }

    public virtual void ActivateUpgrade(UpgradeTypesDatabase upgradeToActivate)
    {
        Logger.Log($"[{this.name}] - Activating Upgrade.", this);
        // ParticleSystem abilityFX = GetComponentInChildren<ParticleSystem>();
        UpgradeTypes upgradeType = upgradeToActivate.First().Value.First().Key;
        Queue<UpgradeLevelData> levelQueueData = upgradeToActivate.First().Value.First().Value;
        float upgradeValue = levelQueueData.Peek().newValue;
        
        switch (upgradeType)
        {
            case UpgradeTypes.DamageUp:
                damage = upgradeValue;
                break;
            case UpgradeTypes.AnimSpeed:
                if (this.abilityName == AbilityNames.FingerShot)
                {
                    // Logic to increase animation speed.  Default value is 1.
                    Animator fingerFlickAnimator = GetComponentInParent<UpgradeManager>().SmallHandsAnimator;
                    fingerFlickAnimator.SetFloat("FingerShot", upgradeValue);
                    Logger.Log($"[{this.name}] -Updating Anim Speed to {upgradeValue}", this);
                }
                break;
            case UpgradeTypes.AttackSpeed: // Reduces activationDelay.
                UpgradeActivationDelay(upgradeValue);
                break;
            // case UpgradeTypes.FireRateUp:
            //     ParticleSystem.EmissionModule emissionModule = abilityFX.emission;
            //     emissionModule.rateOverTime = fireRate = upgradeValue;
            //     break;                
        }

        // if (upgradeType == UpgradeTypes.DamageUp)
        // {
        //     damage = upgradeValue;
        // }
        // else if (upgradeType == UpgradeTypes.FireRateUp)
        // {
        //     ParticleSystem.EmissionModule emissionModule = abilityFX.emission;
        //     emissionModule.rateOverTime = fireRate = upgradeValue;
        // }   
        // else if (upgradeType == UpgradeTypes.AttackSpeed)
        // {
        //     UpgradeActivationDelay(upgradeValue);
        // }
    }
    #endregion

    #region Protected Virtual Methods
    protected virtual void InitializeAbilityData()
    {
        Logger.Log($"[{this.name}] - Initializing ability data OnAwake.", this);
        foreach (var stats in abilityData.abilityStatsArray)
        {
            if (transform.name.Contains(stats.abilityName.ToString()))
            {
                abilityName = stats.abilityName;
                damage = stats.damage;
                fireRate = stats.fireRate;
            }
        }        
    }    
    #endregion
}
