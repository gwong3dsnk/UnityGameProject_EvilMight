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
    protected float activationDelay;
    protected bool isEffectRepeating = false;
    protected GameObject[] playerSockets;
    protected Vector3 enemyPosition = new Vector3();  
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
    [SerializeField] protected AbilityBase playerAbilities;
    #endregion

    protected virtual void Awake()
    {
        InitializeAbilityData();
    }

    public abstract void ActivateAbility(AbilityBase ability);

    public abstract void HandlePlayAnimEventFX();

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
}
