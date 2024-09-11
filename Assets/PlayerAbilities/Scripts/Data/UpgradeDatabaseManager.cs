using System.Collections;
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

    private void Awake()
    {
        InitializeUpgradeDatabase();
    }

    private void InitializeUpgradeDatabase()
    {
        Logger.Log("Initializing UpgradeDatabase on Awake", this);
        foreach (var ability in PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities)
        {
            bool isAbilityInDatabase = upgradeDatabase.TryGetValue(ability.AbilityName, out var value);

            if (!isAbilityInDatabase)
            {
                Logger.Log("New ability detected.  Adding to upgradeTypeDatabase", this);
                foreach (var data in upgradeLibraryData.upgradeStatsData)
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

                            upgradeTypeDictionary.Add(type.upgradeType, typeLevelData);
                        }

                        upgradeDatabase.Add(data.parentAbility, upgradeTypeDictionary);
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

    public void ProcessDequeue(Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue)
    {
        if (upgradeToDequeue.Count != 0)
        {
            upgradeDatabase.TryGetValue(upgradeToDequeue.First().Key, out var typeDictionary);
            typeDictionary.TryGetValue(upgradeToDequeue.First().Value, out var levelData);
            levelData.Dequeue();
        }
    }    
}
