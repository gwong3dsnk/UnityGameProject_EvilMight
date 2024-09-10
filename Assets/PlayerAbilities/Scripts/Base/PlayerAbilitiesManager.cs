using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class PlayerAbilitiesManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerAbilities currentPlayerAbility;
    private List<PlayerAbilities> activeAbilities = new List<PlayerAbilities>();
    public List<PlayerAbilities> ActiveAbilities => activeAbilities;
    private UpgradeTypesDatabase activeUpgrades = new UpgradeTypesDatabase();
    public UpgradeTypesDatabase ActiveUpgrades => activeUpgrades;
    private Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue = new Dictionary<AbilityNames, UpgradeTypes>();
    public Dictionary<AbilityNames, UpgradeTypes> UpgradeToDequeue => upgradeToDequeue;
    public static PlayerAbilitiesManager AbilityManagerInstance { get; private set; }
    public event EventHandler OnActivationCompletion;

    private void Awake()
    {
        // Initialize singleton instance
        if (AbilityManagerInstance == null)
        {
            AbilityManagerInstance = this;
        }
        else if (AbilityManagerInstance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddAbility(PlayerAbilities ability)
    {
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            ability.ActivateAbility();
        }
    }

    public void RemoveAbility(PlayerAbilities ability)
    {
        if (activeAbilities.Contains(ability))
        {
            activeAbilities.Remove(ability);
            ability.DeactivateAbility();
        }
    }

    public void InstantiateAbility(GameObject ability)
    {
        Vector3 particleSpawnPosition = player.transform.position;
        GameObject abilityGameObject = Instantiate(ability, particleSpawnPosition, Quaternion.identity, transform);
        currentPlayerAbility = abilityGameObject.GetComponent<PlayerAbilities>();
        AddAbility(currentPlayerAbility);

        InvokeOnActivationCompletion();
    }    

    public void AddAbilityUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        // TODO: THe follow ability name, upgrade type and most importantly level data needs to be removed from the official 
        // UpgradeTypesDatabase before the next player level up event.
        AbilityNames newAbilityName = newUpgrade.First().Key;
        UpgradeTypes newUpgradeType = newUpgrade.First().Value.First().Key;
        Queue<UpgradeLevelData> newQueue = newUpgrade.First().Value.First().Value;

        if (activeUpgrades.ContainsKey(newAbilityName))
        {
            if (!activeUpgrades[newAbilityName].ContainsKey(newUpgradeType))
            {
                // Ability exists, upgrade type doesn't.  Add upgrade type.
                activeUpgrades[newAbilityName].Add(newUpgradeType, newQueue);
            }
        }
        else
        {
            // First upgrade unlocked for existing ability.  Adding the entry.
            activeUpgrades.Add(newAbilityName, newUpgrade.First().Value);
        }

        upgradeToDequeue.Add(newAbilityName, newUpgradeType);
    }

    public void BeginUpgradeActivation(UpgradeTypesDatabase newUpgrade)
    {
        AbilityNames newAbilityName = newUpgrade.First().Key;

        foreach (PlayerAbilities ability in activeAbilities)
        {
            if (ability.AbilityName == newAbilityName)
            {
                ability.ActivateUpgrade(newUpgrade);
            }
        }

        InvokeOnActivationCompletion();
    }

    private void InvokeOnActivationCompletion()
    {
        OnActivationCompletion?.Invoke(this, EventArgs.Empty);
    }
}
