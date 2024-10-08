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

    public List<UpgradeTypesDatabase> StartGeneratingUpgradeCards()
    {
        Logger.Log("Starting to generate upgrade cards.", this);
        List<UpgradeTypesDatabase> newUpgrades = CreateUpgradesList();

        Logger.Log("Upgrade generation done. Start logging newUpgrades content:", this);
        foreach (var item in newUpgrades) // log
        {
            Logger.Log($"newUpgrades Content - [{item.First().Key}, {item.First().Value.First().Key}]", this);
        }
        Logger.Log("Done logging newUpgrades content");

        return newUpgrades;
    }

    /// <summary>
    /// Selects 3 random upgrades from the UpgradeDatabase and returns them as a list.
    /// </summary>
    /// <returns></returns>
    private List<UpgradeTypesDatabase> CreateUpgradesList()
    {
        List<UpgradeTypesDatabase> chosenUpgradeList = new List<UpgradeTypesDatabase>();
        int x = CalculateNumExistingUpgrades();

        if (x == 1 || x == 2)
        {
            Logger.Log("Only 1 or 2 valid upgrades found.  Storing data into a list and returning.", this);
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
            Logger.LogError("Upgrade database is empty.");
            return null;
        }
        else
        {
            Logger.Log("More than 3 upgrades available.  Starting process to choose 3 random upgrades", this);

            while (x < 3)
            {
                Dictionary<UpgradeTypes, Queue<UpgradeLevelData>> chosenTypes = new Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>();
                UpgradeTypesDatabase chosenUpgrade = new UpgradeTypesDatabase();

                AbilityNames randomAbilityName = ChooseRandomAbilityName();
                upgradeDatabaseManager.UpgradeDatabase.TryGetValue(randomAbilityName, out var typeDictionary);

                UpgradeTypes randomUpgradeType = ChooseRandomUpgradeType(typeDictionary);
                typeDictionary.TryGetValue(randomUpgradeType, out var levelQueue);

                chosenTypes.Add(randomUpgradeType, levelQueue);
                chosenUpgrade.Add(randomAbilityName, chosenTypes);
                chosenUpgradeList.Add(chosenUpgrade);

                x++;
            }

            return chosenUpgradeList;
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