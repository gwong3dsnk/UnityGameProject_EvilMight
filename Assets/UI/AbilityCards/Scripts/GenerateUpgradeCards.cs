using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class GenerateUpgradeCards : MonoBehaviour
{
    private UpgradeTypesDatabase upgradeTypeDatabase;
    private UpgradeLibraryData upgradeLibraryData;

    public List<UpgradeTypesDatabase> StartGeneratingUpgradeCards(UpgradeLibraryData upgradeLibraryData)
    {
        this.upgradeLibraryData = upgradeLibraryData;
        ProcessDequeue();
        // CreateListOfAvailableUpgrades(); // TODO: Should only be called once at game start
        List<UpgradeTypesDatabase> newUpgrades = CreateUpgradesList();

        return newUpgrades;
    }

    private void Start()
    {
        InitializeUpradeDatabase();
    }

    private void InitializeUpradeDatabase()
    {
        foreach (var ability in PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities)
        {
            bool isAbilityInDatabase = upgradeTypeDatabase.TryGetValue(ability.AbilityName, out var value);

            if (!isAbilityInDatabase)
            {
                Logger.Log("New ability detected.  Adding to upgradeTypeDatabase", this);
                foreach (var data in this.upgradeLibraryData.upgradeStatsData)
                {
                    if (data.parentAbility == ability.AbilityName)
                    {
                        Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> upgradeTypeDictionary = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>();

                        foreach (UpgradeTypeData type in data.upgradeType)
                        {
                            Queue<UpgradeLevelData> typeLevelData = new Queue<UpgradeLevelData>();

                            foreach (UpgradeLevelData levelData in type.levelData)
                            {
                                typeLevelData.Enqueue(levelData);
                            }
                        }

                        upgradeTypeDatabase.Add(data.parentAbility, upgradeTypeDictionary);
                    }
                }
            }
            else
            {
                Logger.Log("Ability exists in database.  Removing any where the Queue count is 0.", this);
                foreach (var kvp in value)
                {
                    if (kvp.Value.Count == 0)
                    {
                        value.Remove(kvp.Key);
                    }
                }
            }
        }
    }

    private void ProcessDequeue()
    {
        Dictionary<AbilityNames, UpgradeTypes> dataToDequeue = PlayerAbilitiesManager.AbilityManagerInstance.UpgradeToDequeue;
        if (dataToDequeue.Count != 0)
        {
            upgradeTypeDatabase.TryGetValue(dataToDequeue.First().Key, out var typeDictionary);
            typeDictionary.TryGetValue(dataToDequeue.First().Value, out var levelData);
            levelData.Dequeue();
        }
    }    

    private List<UpgradeTypesDatabase> CreateUpgradesList()
    {
        int validQueueCount = CardUtilityMethods.GetNumValidLevelQueues(upgradeTypeDatabase);

        int x = validQueueCount == 0 ? 3 : 
            (validQueueCount == 1) ? 2 :
            (validQueueCount == 2) ? 1 : 0;

        if (x < 3)
        {
            List<UpgradeTypesDatabase> chosenUpgradeList = new List<UpgradeTypesDatabase>();

            Logger.Log("Starting process to choose 3 random upgrades with existing level queues", this);

            while (x < 3)
            {
                Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> chosenTypes = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>();
                UpgradeTypesDatabase chosenUpgrade = new UpgradeTypesDatabase();

                AbilityNames randomAbilityName = ChooseRandomAbilityName();
                upgradeTypeDatabase.TryGetValue(randomAbilityName, out var typeDictionary);

                UpgradeTypes randomUpgradeType = ChooseRandomUpgradeType(typeDictionary);
                typeDictionary.TryGetValue(randomUpgradeType, out var levelQueue);

                chosenTypes.Add(randomUpgradeType, levelQueue);
                chosenUpgrade.Add(randomAbilityName, chosenTypes);
                chosenUpgradeList.Add(chosenUpgrade);
            }

            return chosenUpgradeList;
        }
        else
        {
            Logger.Log("x is greater than 3 which should never be possible", this);
            return null;
        }
    }

    private AbilityNames ChooseRandomAbilityName()
    {
        List<AbilityNames> abilityNameKeys = upgradeTypeDatabase.Keys.ToList();
        int abilityIndex = GeneralUtilityMethods.GenerateRandomIndex(abilityNameKeys.Count);
        return abilityNameKeys[abilityIndex];
    }

    private UpgradeTypes ChooseRandomUpgradeType(Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> typeDictionary)
    {
        List<UpgradeTypes> upgradeTypeKeys = typeDictionary.Keys.ToList();
        int typeIndex = GeneralUtilityMethods.GenerateRandomIndex(upgradeTypeKeys.Count);
        return upgradeTypeKeys[typeIndex];
    }
}