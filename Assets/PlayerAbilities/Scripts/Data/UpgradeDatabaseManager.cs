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
    public UpgradeTypesDatabase UpgradeDatabase => upgradeDatabase;
    private UpgradeTypesDatabase upgradeDatabase = new UpgradeTypesDatabase();

    private void Start()
    {
        if (upgradeLibraryData == null)
        {
            Logger.LogError("[UpgradeDatabaseManager] - Missing reference to UpgradeLibraryData", this);
        }

        CheckIfUpgradesInDatabase();
    }    

    public void CheckIfUpgradesInDatabase()
    {
        foreach (var ability in AbilitiesManager.AbilityManagerInstance.ActiveAbilities)
        {
            Logger.Log($"[UpgradeDatabaseManager] - Processing upgrade database addition for active ability [{ability.AbilityName}]", this);
            bool isAbilityInDatabase = upgradeDatabase.TryGetValue(ability.AbilityName, out var value);

            if (!isAbilityInDatabase)
            {
                AddAbilityUpgradesToDatabase(ability);
            }
        }

        Logger.Log("[UpgradeDatabaseManager] - Finished updating UpgradeDatabase", this);       
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

    private void CleanEmptyLevelQueueData(Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> value)
    {
        foreach (var kvp in value)
        {
            if (kvp.Value.Count == 0)
            {
                Logger.Log("[UpgradeDatabaseManager] - Empty queue count found.  Removing ability's upgradeType entry.", this);
                value.Remove(kvp.Key); // Need to remove from the upgradeDatabase.
            }
        }
    }    

    public void  ProcessDequeue(Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue)
    {
        if (upgradeToDequeue.Count != 0)
        {
            Logger.Log($"[UpgradeDatabaseManager] - Starting UpgradeDatabaseManager.ProcessDequeue", this);
            AbilityNames abilityName = upgradeToDequeue.First().Key;
            UpgradeTypes upgradeType = upgradeToDequeue.First().Value;

            if (upgradeDatabase.TryGetValue(abilityName, out var typeDictionary))
            {
                if (typeDictionary.TryGetValue(upgradeType, out var levelData))
                {
                    levelData.Dequeue();

                    // Clean up UpgradeDatabase if upgrade is fully unlocked.
                    if (levelData.Count == 0)
                    {
                        typeDictionary.Remove(upgradeType);

                        if (typeDictionary.Count == 0)
                        {
                            upgradeDatabase.Remove(abilityName);
                        }
                    }
                }
            }
        }
    }    
}
