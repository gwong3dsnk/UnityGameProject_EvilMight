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

    private void Start()
    {
        UpdateUpgradeDatabase();
    }    

    public void UpdateUpgradeDatabase()
    {
        Logger.Log("[UpgradeDatabaseManager] - Starting to update UpgradeDatabase.", this);
        foreach (var ability in AbilitiesManager.AbilityManagerInstance.ActiveAbilities)
        {
            Logger.Log($"[UpgradeDatabaseManager] - Processing upgrade database addition for active ability [{ability.AbilityName}]", this);
            bool isAbilityInDatabase = upgradeDatabase.TryGetValue(ability.AbilityName, out var value);

            if (!isAbilityInDatabase)
            {
                AddAbilityUpgradesToDatabase(ability);
            }
            else
            {
                CleanEmptyLevelQueueData(ability, value);
            }
        }

        Logger.Log("[UpgradeDatabaseManager] - Finished updating UpgradeDatabase", this);
        Logger.Log("[UpgradeDatabaseManager] - UpgradeDatabase Contents:", this);
        foreach (var item in upgradeDatabase)
        {
            foreach(var kvp in item.Value)
            {
                Logger.Log($"[UpgradeDatabaseManager] - Ability - {item.Key}, Upgrade - {kvp.Key}", this);
            }
        }
        Logger.Log("[UpgradeDatabaseManager] - End Logging", this);         
    }

    private void AddAbilityUpgradesToDatabase(AbilityBase ability)
    {
        Logger.Log($"[UpgradeDatabaseManager] - Active ability [{ability.AbilityName}]'s upgrades are not in upgrade database.  Adding upgrades", this);
        foreach (var data in upgradeLibraryData.upgradeStatsData)
        {
            if (data.parentAbility == ability.AbilityName)
            {
                Logger.Log($"[UpgradeDatabaseManager] - [{ability.AbilityName}] match found in upgrade library data asset.", this);
                Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> upgradeTypeDictionary = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>();
                foreach (UpgradeTypeData type in data.upgradeType)
                {
                    Queue<UpgradeLevelData> typeLevelData = new Queue<UpgradeLevelData>();

                    foreach (UpgradeLevelData levelData in type.levelData)
                    {
                        Logger.Log($"[UpgradeDatabaseManager] - Enqueuing leveldata for {type.upgradeType}", this);
                        typeLevelData.Enqueue(levelData);
                    }

                    Logger.Log($"[UpgradeDatabaseManager] - Adding Dictionary<{type.upgradeType}, LevelQueue> to upgradeTypeDictionary.", this);
                    upgradeTypeDictionary.Add(type.upgradeType, typeLevelData);
                }

                Logger.Log("[UpgradeDatabaseManager] - Adding upgradeTypeDictionary to upgradeDatabase.", this);
                upgradeDatabase.Add(data.parentAbility, upgradeTypeDictionary);
            }
        }
    }

    private void CleanEmptyLevelQueueData(AbilityBase ability, Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> value)
    {
        Logger.Log($"[UpgradeDatabaseManager] - Active ability [{ability.AbilityName}] is in database.", this);
        foreach (var kvp in value)
        {
            Logger.Log($"[UpgradeDatabaseManager] - Checking [{ability.AbilityName}], [{kvp.Key}] for empty Queue count.", this);
            if (kvp.Value.Count == 0)
            {
                Logger.Log("[UpgradeDatabaseManager] - Empty queue count found.  Removing ability's upgradeType entry.", this);
                value.Remove(kvp.Key);
            }
        }
    }    

    public void ProcessDequeue(Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue)
    {
        if (upgradeToDequeue.Count != 0)
        {
            Logger.Log($"[UpgradeDatabaseManager] - Starting UpgradeDatabaseManager.ProcessDequeue", this);
            upgradeDatabase.TryGetValue(upgradeToDequeue.First().Key, out var typeDictionary);
            typeDictionary.TryGetValue(upgradeToDequeue.First().Value, out var levelData);
            Logger.Log($"[UpgradeDatabaseManager] - First Queue item BEFORE removal - {levelData.Peek()}");
            levelData.Dequeue();
            Logger.Log($"[UpgradeDatabaseManager] - First Queue item AFTER removal - {levelData.Peek()}");
        }
    }    
}
