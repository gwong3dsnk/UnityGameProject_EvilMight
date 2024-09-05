using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public virtual void ActivateUpgrade(Dictionary<string, AbilityUpgrades> upgrade)
    {
        KeyValuePair<string, AbilityUpgrades> upgradeKVP = upgrade.First();
        ParticleSystem singleShotFX = GetComponentInChildren<ParticleSystem>();
        string compressedName = upgradeKVP.Key.Replace(" ", ""); // Remove any spaces.
        string upgradesAbilityName = AbilityUtilityMethods.FormatAbilityName(compressedName);

        foreach (PlayerAbilities ability in PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities)
        {
            if (ability.name.Contains(upgradesAbilityName))
            {
                if (upgradeKVP.Value.upgradeType == UpgradeTypes.DamageUp)
                {
                    ability.Damage = upgradeKVP.Value.newValue;
                }
                else if (upgradeKVP.Value.upgradeType == UpgradeTypes.FireRateUp)
                {
                    ParticleSystem.EmissionModule singleShotEmission = singleShotFX.emission;
                    singleShotEmission.rateOverTime = upgradeKVP.Value.newValue;
                }
            }
        }        
    }

}
