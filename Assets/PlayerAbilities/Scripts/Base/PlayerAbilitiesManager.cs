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
    [SerializeField] AbilityDatabaseManager abilityDatabaseManager;
    [SerializeField] UpgradeDatabaseManager upgradeDatabaseManager;
    private List<PlayerAbilities> activeAbilities = new List<PlayerAbilities>();
    public List<PlayerAbilities> ActiveAbilities => activeAbilities;
    private UpgradeTypesDatabase activeUpgrades = new UpgradeTypesDatabase();
    public UpgradeTypesDatabase ActiveUpgrades => activeUpgrades;
    public static PlayerAbilitiesManager AbilityManagerInstance { get; private set; }
    public event EventHandler OnActivationCompletion;

    private void Awake()
    {
        Logger.Log($"Initializing PLAYER ABILITY MANAGER singleton instance OnAwake.", this);
        if (AbilityManagerInstance == null)
        {
            AbilityManagerInstance = this;
        }
        else if (AbilityManagerInstance != this)
        {
            Destroy(gameObject);
        }

        if (abilityDatabaseManager == null || upgradeDatabaseManager == null)
        {
            Logger.LogError("Missing references to either ability or upgrade database manager scripts.", this);
        }
    }

    private void OnEnable() 
    {
        ActivateButtonOnClick.OnAbilityChosen += InstantiateAbility;
        ActivateButtonOnClick.OnUpgradeChosen += AddAbilityUpgrade;
    }

    private void OnDisable()
    {
        ActivateButtonOnClick.OnAbilityChosen -= InstantiateAbility;
        ActivateButtonOnClick.OnUpgradeChosen -= AddAbilityUpgrade;
    }

    public void AddAbility(PlayerAbilities ability)
    {
        Logger.Log($"Adding ability [{ability.AbilityName}] to activeAbilities.", this);
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            ability.ActivateAbility(ability);
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
        GameObject abilityGameObject = Instantiate(ability, transform.position, Quaternion.identity, transform);
        PlayerAbilities currentPlayerAbility = abilityGameObject.GetComponent<PlayerAbilities>();
        AddAbility(currentPlayerAbility);

        // Remove the unlocked ability from the ability database so it won't be shown in future level-ups.
        abilityDatabaseManager.RemoveAbilityFromDatabase(currentPlayerAbility);

        InvokeOnActivationCompletion();
    }

    public void AddAbilityUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log("Starting AddAbilityUpgrade", this);
        Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue = new Dictionary<AbilityNames, UpgradeTypes>();
        AbilityNames newAbilityName = newUpgrade.First().Key;
        UpgradeTypes newUpgradeType = newUpgrade.First().Value.First().Key;
        Queue<UpgradeLevelData> newQueue = newUpgrade.First().Value.First().Value;

        if (activeUpgrades.ContainsKey(newAbilityName))
        {
            if (!activeUpgrades[newAbilityName].ContainsKey(newUpgradeType))
            {
                Logger.Log("Ability exists in ActiveUpgrades, upgrade type doesn't.  Adding only upgrade entry.", this); 
                activeUpgrades[newAbilityName].Add(newUpgradeType, newQueue);
            }
            else
            {
                Logger.Log("Ability exists in ActiveUpgrades, upgrade type exists.  Don't need to do anything.", this);
            }
        }
        else
        {
            Logger.Log("Ability DOES NOT exist in ActiveUpgrades.  Adding ability & upgrade entry.", this); 
            activeUpgrades.Add(newAbilityName, newUpgrade.First().Value);
        }

        Logger.Log($"Upgrade Added: [{newUpgrade.First().Key}] + [{newUpgrade.First().Value.First().Key}]");

        BeginUpgradeActivation(newUpgrade);

        // Remove the upgrade LevelData from the UpgradeDatabase so it won't be displayed again on future level-ups.
        upgradeToDequeue.Add(newAbilityName, newUpgradeType);
        upgradeDatabaseManager.ProcessDequeue(upgradeToDequeue);

        InvokeOnActivationCompletion();        
    }

    public void BeginUpgradeActivation(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log("------------------------------------------------", this);
        Logger.Log("Starting BeginUpgradeActivation", this);
        AbilityNames newAbilityName = newUpgrade.First().Key;

        foreach (PlayerAbilities ability in activeAbilities)
        {
            if (ability.AbilityName == newAbilityName)
            {
                Logger.Log("Ability match found in activeAbilities. Calling ActivateUpgrade", this);
                ability.ActivateUpgrade(newUpgrade);
            }
        }
    }

    private void InvokeOnActivationCompletion()
    {
        OnActivationCompletion?.Invoke(this, EventArgs.Empty);
    }
}
