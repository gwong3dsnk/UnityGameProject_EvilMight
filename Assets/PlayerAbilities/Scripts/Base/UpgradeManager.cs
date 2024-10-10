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
    public UpgradeDatabaseManager UpgradeDatabaseManager => upgradeDatabaseManager;
    [SerializeField] ActivateButtonOnClick activateButtonOnClick;
    AbilityNames nameOfAbilityToUpgrade;
    UpgradeTypesDatabase upgradeToActivate;
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
        activateButtonOnClick.OnUpgradeChosen += BeginUpgradeActivation;        
    }

    private void OnDisable()
    {
        activateButtonOnClick.OnUpgradeChosen -= BeginUpgradeActivation;
    }

    public void BeginUpgradeActivation(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log("------------------------------------------------", this);
        Logger.Log("Starting BeginUpgradeActivation", this);

        nameOfAbilityToUpgrade = newUpgrade.First().Key;
        upgradeToActivate = newUpgrade;

        CheckIfAbilityIsActive(upgradeToActivate);
        SendDataToDequeue(upgradeToActivate);
        InvokeUpgradeActivationCompletionEvent();
    }

    private void CheckIfAbilityIsActive(UpgradeTypesDatabase upgradeToActivate)
    {
        AbilitiesManager abilityManager = GetComponent<AbilitiesManager>();
        bool isAbilityFound = true;

        foreach (AbilityBase ability in abilityManager.ActiveAbilities)
        {
            if (ability.AbilityName == nameOfAbilityToUpgrade)
            {
                Logger.Log("Ability match found in activeAbilities. Calling ActivateUpgrade", this);
                ability.ActivateUpgrade(upgradeToActivate);
                break;
            }
            else
            {
                isAbilityFound = false;
            }
        }

        if (!isAbilityFound)
        {
            Logger.LogError("No matching ability has been found in ActiveAbilities.", this);
        }
    }

    private void SendDataToDequeue(UpgradeTypesDatabase upgradeToActivate)
    {
        Logger.Log("Sending level data to be dequeued.", this);
        Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue = new Dictionary<AbilityNames, UpgradeTypes>();
        UpgradeTypes upgradeType = upgradeToActivate.First().Value.First().Key;

        // Remove the upgrade LevelData from the UpgradeDatabase so it won't be displayed again on future level-ups.
        upgradeToDequeue.Add(nameOfAbilityToUpgrade, upgradeType);
        upgradeDatabaseManager.ProcessDequeue(upgradeToDequeue);
    }

    private void InvokeUpgradeActivationCompletionEvent()
    {
        OnUpgradeActivationCompletion?.Invoke(this, EventArgs.Empty);
    }
}
