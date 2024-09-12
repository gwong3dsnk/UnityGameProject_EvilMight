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
    [SerializeField] protected AbilityLibraryData abilityData; 
    protected ParticleSystem abilityParticleSystem;
    protected float activationDelay;
    protected bool isEffectRepeating = false;
    // Below SerializeFields are just for reference during runtime, not to be set in Inspector.
    [SerializeField] protected AbilityNames abilityName;
    public AbilityNames AbilityName => abilityName;
    [SerializeField] protected string abilityDescription;
    public string AbilityDescription => abilityDescription;
    [SerializeField] protected int damage;
    public int Damage { get => damage; set => damage = value; }
    [SerializeField] protected int fireRate;
    public int FireRate => fireRate;
    [SerializeField] protected GameObject prefab;
    public GameObject Prefab => prefab;
    [SerializeField] protected PlayerAbilities playerAbilities;
    public PlayerAbilities PlayerAbilitiesScript => playerAbilities;

    public virtual void Awake()
    {
        InitializeAbilityData();
    }

    public virtual void ActivateAbility(PlayerAbilities ability)
    {
        // Retrieve the runtime gameobject's particle system component.
        abilityParticleSystem = ability.gameObject.GetComponentInChildren<ParticleSystem>();
        if (abilityParticleSystem == null)
        {
            Logger.LogError("Failed to get ability gameobject's particle system component", this);
        }

        // isEffectRepeating = true only for abilities that will be replyed continuously until level end with n second delays.
        if (isEffectRepeating)
        {
            StartCoroutine(ReplayActivation());
        }
        else
        {
            if (abilityParticleSystem != null)
            {
                abilityParticleSystem.Play();
            }
        }
    }

    protected virtual IEnumerator ReplayActivation()
    {
        while(true)
        {
            if (abilityParticleSystem != null)
            {
                abilityParticleSystem.Play();
            }

            ExecuteSecondaryActivationBehavior();

            yield return new WaitForSeconds(activationDelay);
            DeactivateAbility();
        }
    }

    public virtual void DeactivateAbility()
    {
        if (abilityParticleSystem != null)
        {
            Logger.Log("Deactivating Ability");
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
                prefab = stats.prefab;
                playerAbilities = stats.playerAbilities;
            }
        }        
    }    

    public virtual void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log($"Activating Upgrade in - {this.name} PlayerAbilities.", this);
        UpgradeTypes newUpgradeType = newUpgrade.First().Value.First().Key;
        Queue<UpgradeLevelData> newQueue = newUpgrade.First().Value.First().Value;
        int newValue = newQueue.Peek().newValue;
        ParticleSystem abilityFX = GetComponentInChildren<ParticleSystem>();
        
        if (newUpgradeType == UpgradeTypes.DamageUp)
        {
            damage = newValue;
        }
        else if (newUpgradeType == UpgradeTypes.FireRateUp)
        {
            ParticleSystem.EmissionModule emissionModule = abilityFX.emission;
            emissionModule.rateOverTime = newValue;
        }   
    }

    protected abstract void ExecuteSecondaryActivationBehavior();
}
