using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class UpgradeDatabaseManager : MonoBehaviour
{
    [SerializeField] UpgradeLibraryData upgradeLibraryData;
    private UpgradeTypesDatabase upgradeDatabase = new UpgradeTypesDatabase();
    public UpgradeTypesDatabase UpgradeDatabase => upgradeDatabase;

    // private void Awake()
    // {
    //     InitializeUpgradeDatabase();
    // }

    private void Start()
    {
        InitializeUpgradeDatabase();
    }    

    private void InitializeUpgradeDatabase()
    {
        Logger.Log("Initializing UPGRADE DATABASE MANAGER OnAwake", this);
        foreach (var ability in PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities)
        {
            Logger.Log($"Processing upgrade database addition for active ability [{ability.AbilityName}]", this);
            bool isAbilityInDatabase = upgradeDatabase.TryGetValue(ability.AbilityName, out var value);

            if (!isAbilityInDatabase)
            {
                Logger.Log($"Active ability [{ability.AbilityName}]'s upgrades are not in upgrade database.  Adding upgrades", this);
                foreach (var data in upgradeLibraryData.upgradeStatsData)
                {
                    if (data.parentAbility == ability.AbilityName)
                    {
                        Logger.Log($"[{ability.AbilityName}] match found in upgrade library data asset.", this);
                        Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> upgradeTypeDictionary = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>();
                        foreach (UpgradeTypeData type in data.upgradeType)
                        {
                            Queue<UpgradeLevelData> typeLevelData = new Queue<UpgradeLevelData>();

                            foreach (UpgradeLevelData levelData in type.levelData)
                            {
                                Logger.Log($"Enqueuing leveldata for {type.upgradeType}", this);
                                typeLevelData.Enqueue(levelData);
                            }

                            Logger.Log($"Adding Dictionary<{type.upgradeType}, LevelQueue> to upgradeTypeDictionary.", this);
                            upgradeTypeDictionary.Add(type.upgradeType, typeLevelData);
                        }

                        Logger.Log("Adding upgradeTypeDictionary to upgradeDatabase.", this);
                        upgradeDatabase.Add(data.parentAbility, upgradeTypeDictionary);
                    }
                }
            }
            else
            {
                Logger.Log($"Active ability [{ability.AbilityName}] is in database.", this);
                foreach (var kvp in value)
                {
                    Logger.Log($"Checking [{ability.AbilityName}], [{kvp.Key}] for empty Queue count.", this);
                    if (kvp.Value.Count == 0)
                    {
                        Logger.Log("Empty queue count found.  Removing ability's upgradeType entry.", this);
                        value.Remove(kvp.Key);
                    }
                }
            }
        }

        Logger.Log("Finished upgrade database initialization", this);
        Logger.Log("Logging Initialized Upgrade Database Contents:", this);
        foreach (var item in upgradeDatabase)
        {
            foreach(var kvp in item.Value)
            {
                Logger.Log($"Ability - {item.Key}, Upgrade - {kvp.Key}", this);
            }
        }
        Logger.Log("End Logging", this);         
    }

    public void ProcessDequeue(Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue)
    {
        if (upgradeToDequeue.Count != 0)
        {
            Logger.Log($"Starting UpgradeDatabaseManager.ProcessDequeue", this);
            upgradeDatabase.TryGetValue(upgradeToDequeue.First().Key, out var typeDictionary);
            typeDictionary.TryGetValue(upgradeToDequeue.First().Value, out var levelData);
            Logger.Log($"First Queue item BEFORE removal - {levelData.Peek()}");
            levelData.Dequeue();
            Logger.Log($"First Queue item AFTER removal - {levelData.Peek()}");
        }
    }    
}
