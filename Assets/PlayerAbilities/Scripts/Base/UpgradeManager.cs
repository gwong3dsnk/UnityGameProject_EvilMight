using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] UpgradeDatabaseManager upgradeDatabaseManager;
    [SerializeField] ActivateButtonOnClick activateButtonOnClick;         
    private UpgradeTypesDatabase activeUpgrades = new UpgradeTypesDatabase();
    public static UpgradeManager UpgradeManagerInstance { get; private set; }    
    public event EventHandler OnUpgradeActivationCompletion;

    private void Awake()
    {
        if (UpgradeManagerInstance == null)
        {
            UpgradeManagerInstance = this;
        }
        else if (UpgradeManagerInstance != this)
        {
            Destroy(gameObject);
        }

        if (upgradeDatabaseManager == null)
        {
            Logger.LogError("[UpgradeManager] - Missing reference to upgrade database manager scripts.", this);
        }        
    }

    private void OnEnable()
    {
        activateButtonOnClick.OnUpgradeChosen += AddAbilityUpgrade;        
    }

    private void OnDisable()
    {
        activateButtonOnClick.OnUpgradeChosen -= AddAbilityUpgrade;        
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

        OnUpgradeActivationCompletion?.Invoke(this, EventArgs.Empty);
    }

    public void BeginUpgradeActivation(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log("------------------------------------------------", this);
        Logger.Log("Starting BeginUpgradeActivation", this);
        AbilitiesManager abilityManager = GetComponent<AbilitiesManager>();
        AbilityNames newAbilityName = newUpgrade.First().Key;

        foreach (AbilityBase ability in abilityManager.ActiveAbilities)
        {
            if (ability.AbilityName == newAbilityName)
            {
                Logger.Log("Ability match found in activeAbilities. Calling ActivateUpgrade", this);
                ability.ActivateUpgrade(newUpgrade);
            }
        }
    }    
}
