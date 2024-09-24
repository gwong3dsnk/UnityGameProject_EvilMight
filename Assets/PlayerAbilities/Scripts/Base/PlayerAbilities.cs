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
    protected GameObject[] playerSockets;
    protected Vector3 enemyPosition = new Vector3();
    public Vector3 EnemyPosition => enemyPosition;    
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

    protected virtual void Awake()
    {
        InitializeAbilityData();
        playerSockets = AbilityUtilityMethods.GetPlayerSockets();
    }

    public virtual void ActivateAbility(PlayerAbilities ability)
    {
        Logger.Log($"Activating {this.name}", this);
        SetParticleSystemLocationToSocket();

        // Retrieve the runtime gameobject's particle system component.
        abilityParticleSystem = ability.gameObject.GetComponentInChildren<ParticleSystem>();
        if (abilityParticleSystem == null)
        {
            Logger.LogWarning("Failed to get ability gameobject's particle system component", this);
        }
    }

    protected IEnumerator ReplayAbilityFX()
    {
        while(true)
        {
            Logger.Log($"Executing replay activation loop for {this.name}");
            if (abilityParticleSystem != null)
            {
                abilityParticleSystem.Play();
            }

            ExecuteSecondaryActivationBehavior();

            yield return new WaitForSeconds(activationDelay);
            DeactivateAbility();
        }
    }

    protected IEnumerator ReplayAnimation()
    {
        Logger.Log("ReplayAnimation area");
        yield return new WaitForSeconds(activationDelay);
    }

    public virtual void DeactivateAbility()
    {
        Logger.Log($"Deactivating {this.name}.", this);
        if (abilityParticleSystem != null)
        {
            abilityParticleSystem.Stop();
        }
    }

    protected virtual void InitializeAbilityData()
    {
        Logger.Log($"Initializing {this.name}'s ability data OnAwake.", this);
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
        Logger.Log($"Activating Upgrade for {this.name}.", this);
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

    protected abstract void SetParticleSystemLocationToSocket();
}
