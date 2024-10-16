using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class GenerateUpgradeCards : MonoBehaviour
{
    [SerializeField] UpgradeDatabaseManager upgradeDatabaseManager;
    private List<UpgradeTypesDatabase> chosenUpgradeList = new List<UpgradeTypesDatabase>();
    private AbilityNames randomAbilityName;
    private UpgradeTypes randomUpgradeType;

    public List<UpgradeTypesDatabase> StartGeneratingUpgradeCards()
    {
        Logger.Log($"[{this.name}] - Starting to generate upgrade cards.", this);
        chosenUpgradeList.Clear();
        chosenUpgradeList = CreateUpgradesList();

        // Logger.Log($"[{this.name}] - Upgrade generation done. Start logging chosenUpgradeList content:", this);
        // foreach (var item in chosenUpgradeList) // log
        // {
        //     Logger.Log($"[{this.name}] - chosenUpgradeList Content - [{item.First().Key}, {item.First().Value.First().Key}]", this);
        // }
        // Logger.Log($"[{this.name}] - Done logging newUpgrades content");

        return chosenUpgradeList;
    }

    /// <summary>
    /// Selects 3 random upgrades from the UpgradeDatabase and returns them as a list.
    /// </summary>
    /// <returns></returns>
    private List<UpgradeTypesDatabase> CreateUpgradesList()
    {
        int x = CalculateNumExistingUpgrades();

        if (x == 1 || x == 2)
        {
            // Logger.Log($"[{this.name}] - Only 1 or 2 valid upgrades found.  Storing data into a list and returning.", this);
            // Less than 3 upgrades exist, store into list and return
            foreach (var kvp in upgradeDatabaseManager.UpgradeDatabase)
            {
                foreach (var type in kvp.Value)
                {
                    Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> chosenTypes = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>
                    {
                        { type.Key, type.Value }
                    };
                    UpgradeTypesDatabase chosenUpgrade = new UpgradeTypesDatabase() { { kvp.Key, chosenTypes } };
                    chosenUpgradeList.Add(chosenUpgrade);
                }
            }

            return chosenUpgradeList;
        }
        else if (x == 3)
        {
            Logger.LogError($"[{this.name}] - Upgrade database is empty.");
            return null;
        }
        else
        {
            Logger.Log($"[{this.name}] - More than 3 upgrades available.  Starting process to choose 3 random upgrades", this);

            while (x < 3)
            {
                Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> chosenTypes = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>();
                UpgradeTypesDatabase chosenUpgrade = new UpgradeTypesDatabase();
                bool isAbilityUpgradeDup = true;

                while (isAbilityUpgradeDup)
                {
                    Logger.Log($"[{this.name}] - Generating random ability and upgrade.", this);
                    Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> typeDictionary = PickRandomAbilityAndUpgrade();

                    if (!DoesDataAlreadyExistInList())
                    {
                        typeDictionary.TryGetValue(randomUpgradeType, out var levelQueue);
                        chosenTypes.Add(randomUpgradeType, levelQueue);
                        chosenUpgrade.Add(randomAbilityName, chosenTypes);
                        isAbilityUpgradeDup = false;
                    }
                }

                chosenUpgradeList.Add(chosenUpgrade);

                x++;
            }

            return chosenUpgradeList;
        }
    }

    private Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> PickRandomAbilityAndUpgrade()
    {
        randomAbilityName = ChooseRandomAbilityName();
        upgradeDatabaseManager.UpgradeDatabase.TryGetValue(randomAbilityName, out var typeDictionary);
        randomUpgradeType = ChooseRandomUpgradeType(typeDictionary);
        return typeDictionary;
    }

    private bool DoesDataAlreadyExistInList()
    {
        Logger.Log($"[{this.name}] - Checking if exists in list.", this);

        if (chosenUpgradeList.Count == 0)
        {
            Logger.Log($"[{this.name}] - chosenUpgradeList is empty, returning false.", this);
            return false;
        }
        else
        {
            Logger.Log($"[{this.name}] - chosenUpgradeList is not empty, looping.", this);
            foreach (var database in chosenUpgradeList)
            {
                if (database.TryGetValue(randomAbilityName, out var upgrades))
                {
                    foreach (var upgrade in upgrades)
                    {
                        if (randomUpgradeType == upgrade.Key)
                        {
                            Logger.Log($"[{this.name}] - Matching Upgrade found, returning true.", this);
                            return true;
                        }
                    }
                }
            }

            Logger.Log($"[{this.name}] - No matching Upgrade found, returning false.", this);
            return false;
        }
    }

    private int CalculateNumExistingUpgrades()
    {
        int validQueueCount = CardUtilityMethods.GetNumValidLevelQueues(upgradeDatabaseManager.UpgradeDatabase);

        int x = validQueueCount == 0 ? 3 :
            (validQueueCount == 1) ? 2 :
            (validQueueCount == 2) ? 1 : 0;
        return x;
    }

    private AbilityNames ChooseRandomAbilityName()
    {
        List<AbilityNames> abilityNameKeys = upgradeDatabaseManager.UpgradeDatabase.Keys.ToList();
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