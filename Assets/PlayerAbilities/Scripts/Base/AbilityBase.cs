using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
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
    [SerializeField] protected ParticleSystem[] particleSystems;
    [SerializeField] [ReadOnly] protected AbilityNames abilityName;  
    [SerializeField] [ReadOnly] protected float damage;   
    [SerializeField] [ReadOnly] protected float attackSpeed;
    [SerializeField] [ReadOnly] protected float animSpeed;
    [SerializeField] [ReadOnly] protected float abilityRange;

    // Public Fields/Properties
    public AbilityNames AbilityName => abilityName;
    public float Damage => damage;

    // Protected Fields
    protected GameObject[] playerSockets;
    protected Vector3 enemyPosition = new Vector3();
    protected bool isAttacking = false; 
    protected Transform player;
    protected const string colliderCompareTag = "Enemy";
    #endregion

    protected virtual void Awake()
    {
        InitializeAbilityData();
        player = FindObjectOfType<PlayerHealth>().transform;
        
        if (player == null)
        {
            Logger.LogError("Missing reference to player transform.", this);
            return;
        }

        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        transform.position = player.position;
        transform.rotation = player.rotation;
    }

    #region Public Abstract Methods
    public abstract void HandleAnimEventFX();
    #endregion

    #region Public Virtual Methods
    public virtual void ActivateAbility()
    {
        UpgradeManager.UpgradeManagerInstance.UpgradeDatabaseManager.CheckIfUpgradesInDatabase();
    }

    public virtual void DeactivateAbility()
    {
        StopAllCoroutines();
    }

    public virtual void ActivateUpgrade(UpgradeTypesDatabase upgradeToActivate)
    {
        Logger.Log($"[{this.name}] - Activating Upgrade.", this);
        UpgradeTypes upgradeType = upgradeToActivate.First().Value.First().Key;
        Queue<UpgradeLevelData> levelQueueData = upgradeToActivate.First().Value.First().Value;
        float upgradeValue = levelQueueData.Peek().newValue;
        
        switch (upgradeType)
        {
            case UpgradeTypes.DamageUp:
                damage = upgradeValue;
                break;
            case UpgradeTypes.AnimSpeed:
                UpgradeAnimationSpeed(upgradeValue);
                animSpeed = upgradeValue;
                break;
            case UpgradeTypes.AttackSpeed:
                UpgradeAttackSpeed(upgradeValue);
                attackSpeed = upgradeValue;
                break;
            // case UpgradeTypes.AbilityRange:
            //     break;
        }
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
                attackSpeed = stats.attackSpeed;
                animSpeed = stats.animSpeed;
                abilityRange = stats.abilityRange;
            }
        }        
    }

    protected virtual void UpgradeAnimationSpeed(float upgradeValue)
    {

    }

    protected virtual void UpgradeAttackSpeed(float upgradeValue)
    {

    } 

    protected virtual Dictionary<Collider, float> GetNearbyEnemies(Vector3 currentPosition, float meleeRange)
    {
        Collider[] hitColliders = Physics.OverlapSphere(currentPosition, meleeRange);
        Dictionary<Collider, float> enemiesWithinRange = new Dictionary<Collider, float>();

        foreach (Collider collider in hitColliders)
        {
            // Isolate the Enemy objects and store them and their distance from this into the dictionary.
            if (collider.CompareTag(colliderCompareTag))
            {
                enemyPosition = collider.transform.position;
                float enemyDistanceFromPlayer = Vector3.Distance(currentPosition, enemyPosition);
                enemiesWithinRange.Add(collider, enemyDistanceFromPlayer);
            }
        }

        return enemiesWithinRange;
    }            
    #endregion
}
