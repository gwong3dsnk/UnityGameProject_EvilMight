using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject defaultStartingAbility;
    [SerializeField] UpgradeLibraryData upgradeLibraryData;
    private PlayerAbilitiesManager abilityManager;

    void Start()
    {
        abilityManager = PlayerAbilitiesManager.AbilityManagerInstance;
        abilityManager.upgradeTypeDatabase = new Dictionary<AbilityNames, Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>>();
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        InstantiateDefaultAbility();
        FindDefaultAbilityUpgrades();
    }

    private void InstantiateDefaultAbility()
    {
        abilityManager.InstantiateAbility(defaultStartingAbility);
    }

    private void FindDefaultAbilityUpgrades()
    {
        string defaultAbilityName = defaultStartingAbility.name;
        foreach (var data in upgradeLibraryData.upgradeStatsData)
        {
            
            if (data.parentAbility.ToString() == defaultAbilityName)
            {
                PopulateDefaultAbilityUpgrades(data, data.parentAbility);
            }
        }
    }

    private void PopulateDefaultAbilityUpgrades(UpgradeLibraryData.UpgradeStats data, AbilityNames abilityName)
    {
        Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> upgradeTypeDictionary = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>();

        foreach (UpgradeTypeData type in data.upgradeType)
        {
            Queue<UpgradeLevelData> typeLevelData = new Queue<UpgradeLevelData>();
            foreach (UpgradeLevelData levelData in type.levelData)
            {
                typeLevelData.Enqueue(levelData);
            }

            upgradeTypeDictionary.Add(type.upgradeType, typeLevelData);
        }

        abilityManager.upgradeTypeDatabase.Add(abilityName, upgradeTypeDictionary);
    }
}
