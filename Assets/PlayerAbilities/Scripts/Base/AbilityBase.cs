using System.Collections;
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
    [SerializeField] protected AbilityLibraryData abilityData; 
    protected bool isEffectRepeating = false;
    protected GameObject[] playerSockets;
    protected Vector3 enemyPosition = new Vector3();  
    // Below SerializeFields are just for reference during runtime, not to be set in Inspector.
    [SerializeField] protected AbilityNames abilityName;
    public AbilityNames AbilityName => abilityName;
    [SerializeField] protected float damage;
    public float Damage { get => damage; set => damage = value; }
    [SerializeField] protected float fireRate;
    public float FireRate => fireRate;
    [SerializeField] protected float attackSpeed;
    #endregion

    protected virtual void Awake()
    {
        InitializeAbilityData();
    }

    public virtual void ActivateAbility()
    {
        UpgradeManager.UpgradeManagerInstance.UpgradeDatabaseManager.UpdateUpgradeDatabase();
    }

    public abstract void HandlePlayAnimEventFX();

    public abstract void UpgradeActivationDelay(float upgradeValue);

    // protected virtual void GetAbilityParticleSystem(PlayerAbilities ability)
    // {
    //     // Retrieve the runtime gameobject's particle system component.
    //     particleSystems = ability.gameObject.GetComponentsInChildren<ParticleSystem>();
    //     if (particleSystems.Length == 0)
    //     {
    //         Logger.LogWarning($"No particle system gameobject components found within [{ability.AbilityName}]", this);
    //     }
    // }

    // protected IEnumerator ReplayAbilityFX()
    // {
    //     while(true)
    //     {
    //         Logger.Log($"Executing replay activation loop for {this.name}");
    //         if (abilityParticleSystem != null)
    //         {
    //             abilityParticleSystem.Play();
    //         }

    //         yield return new WaitForSeconds(activationDelay);
    //         DeactivateAbility();
    //     }
    // }

    // protected IEnumerator ReplayAnimation()
    // {
    //     Logger.Log("ReplayAnimation area");
    //     yield return new WaitForSeconds(activationDelay);
    // }

    public virtual void DeactivateAbility()
    {
        Logger.Log($"Stopping {this.name}'s particle system.", this);
        // if (abilityParticleSystem != null)
        // {
        //     abilityParticleSystem.Stop();
        // }
    }

    protected virtual void InitializeAbilityData()
    {
        Logger.Log($"Initializing {this.name}'s ability data OnAwake.", this);
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

    public virtual void ActivateUpgrade(UpgradeTypesDatabase upgradeToActivate)
    {
        Logger.Log($"Activating Upgrade for {this.name}.", this);
        ParticleSystem abilityFX = GetComponentInChildren<ParticleSystem>();
        UpgradeTypes upgradeType = upgradeToActivate.First().Value.First().Key;
        Queue<UpgradeLevelData> levelQueueData = upgradeToActivate.First().Value.First().Value;
        float upgradeValue = levelQueueData.Peek().newValue;
        
        switch (upgradeType)
        {
            case UpgradeTypes.DamageUp:
                damage = upgradeValue;
                break;
            case UpgradeTypes.FireRateUp:
                ParticleSystem.EmissionModule emissionModule = abilityFX.emission;
                emissionModule.rateOverTime = fireRate = upgradeValue;
                break;
            case UpgradeTypes.AttackSpeed:
                UpgradeActivationDelay(upgradeValue);
                break;
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
}
