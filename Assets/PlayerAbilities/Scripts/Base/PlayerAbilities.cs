using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public abstract class PlayerAbilities : MonoBehaviour
{
    [SerializeField] protected ParticleSystem abilityParticleSystem;
    [SerializeField] protected AbilityLibraryData abilityData;
    // Below SerializeFields are just for reference during runtime, not to be set in Inspector.
    [SerializeField] protected AbilityNames abilityName;
    public AbilityNames AbilityName => abilityName;
    [SerializeField] protected string abilityDescription;
    public string AbilityDescription => abilityDescription;
    [SerializeField] protected int damage;
    public int Damage { get => damage; set => damage = value; }
    [SerializeField] protected int fireRate;
    public int FireRate => fireRate;
    [SerializeField] protected AbilityUpgrades[] abilityUpgrades;
    public AbilityUpgrades[] AbilityUpgrades => abilityUpgrades;
    [SerializeField] protected GameObject prefab;
    public GameObject Prefab => prefab;
    [SerializeField] protected PlayerAbilities playerAbilities;
    public PlayerAbilities PlayerAbilitiesScript => playerAbilities;

    public virtual void Awake()
    {
        InitializeAbilityData();
    }

    public virtual void ActivateAbility()
    {
        if (abilityParticleSystem != null)
        {
            abilityParticleSystem.Play();
        }
    }

    public virtual void DeactivateAbility()
    {
        if (abilityParticleSystem != null)
        {
            abilityParticleSystem.Stop();
        }
    }

    protected virtual void InitializeAbilityData()
    {
        foreach (var stats in abilityData.abilityStatsArray)
        {
            if (transform.name.Contains(stats.abilityName.ToString()))
            {
                abilityName = stats.abilityName;
                abilityDescription = stats.abilityDescription;
                damage = stats.damage;
                fireRate = stats.fireRate;
                abilityUpgrades = stats.abilityUpgrades;
                prefab = stats.prefab;
                playerAbilities = stats.playerAbilities;
            }
        }        
    }    

    public virtual void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log($"This - {this.name}");
        // AbilityNames newAbilityName = newUpgrade.First().Key;
        UpgradeTypes newUpgradeType = newUpgrade.First().Value.First().Key;
        Queue<UpgradeLevelData> newQueue = newUpgrade.First().Value.First().Value;
        int newValue = newQueue.Peek().newValue;
        ParticleSystem abilityFX = GetComponentInChildren<ParticleSystem>();
        Logger.Log($"FX Name - {abilityFX.name}");
        
        if (newUpgradeType == UpgradeTypes.DamageUp)
        {
            Logger.Log("Updating damage value");
            damage = newValue;
        }
        else if (newUpgradeType == UpgradeTypes.FireRateUp)
        {
            Logger.Log("Updating emission value");
            ParticleSystem.EmissionModule emissionModule = abilityFX.emission;
            emissionModule.rateOverTime = newValue;
        }   
    }
}
