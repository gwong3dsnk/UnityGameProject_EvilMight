using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateUpgradeCards : MonoBehaviour
{
    Dictionary<string, AbilityUpgrades> allAvailableUpgrades = new Dictionary<string, AbilityUpgrades>(); // deprecate

    public Dictionary<string, AbilityUpgrades> StartGeneratingUpgradeCards(UpgradeLibraryData upgradeLibraryData)
    {
        CreateListOfAvailableUpgrades(upgradeLibraryData);
        Dictionary<string, AbilityUpgrades> newUpgrades = CreateUpgradesList();

        return newUpgrades;
    }

    private void CreateListOfAvailableUpgrades(UpgradeLibraryData upgradeLibraryData)
    {
        allAvailableUpgrades.Clear();

        foreach (var data in upgradeLibraryData.upgradeStatsData)
        {
            // Query ActiveAbilities, see if AbilityName can be found in Key in upgradeTypeDatabase
            // If not, populate here
            // If it is present, verify that the nested dictionary has upgrade types with leveldata
            // If data is present, do nothing.
            if (PlayerAbilitiesManager.AbilityManagerInstance.ActiveUpgrades.Count > 0)
            {
                Logger.Log("Some abilities have been unlocked", this);
                // for (int i = 0; i < data.upgradeType.Length; i++)
                // {
                //     bool isFound = false;

                //     foreach (KeyValuePair<string, AbilityUpgrades> kvp in PlayerAbilitiesManager.AbilityManagerInstance.ActiveUpgrades)
                //     {
                //         if (kvp.Key.Contains(data.abilityName.ToString()) && data.abilityUpgrades[i].upgradeType != kvp.Value.upgradeType)
                //         {
                //             isFound = true;
                //             break;
                //         }
                //     }

                //     if (!isFound)
                //     {
                //         if (!allAvailableUpgrades.ContainsKey($"{data.abilityName}_idx{i}"))
                //         {
                //             allAvailableUpgrades.Add($"{data.abilityName}_idx{i}", data.abilityUpgrades[i]);
                //         }
                //     }
                // }

                // foreach (var item in allAvailableUpgrades)
                // {
                //     Logger.Log("Contents of AbilityUpgradeData after Generation");
                //     Logger.Log(item.Key);
                //     Logger.Log(item.Value.upgradeType.ToString());
                // }
            }
        }
    }

    private Dictionary<string, AbilityUpgrades> CreateUpgradesList()
    {
        Dictionary<string, AbilityUpgrades> newUpgradeList = new Dictionary<string, AbilityUpgrades>();
        List<string> upgradeKeys = new List<string>();

        if (allAvailableUpgrades.Count <= 3)
        {
            return allAvailableUpgrades;
        }
        else
        {
            int x = 0;

            while (x < 3)
            {
                foreach (KeyValuePair<string, AbilityUpgrades> upgradePair in allAvailableUpgrades)
                {
                    upgradeKeys.Add(upgradePair.Key);
                }

                int index = BaseUtilityMethods.GenerateRandomIndex(allAvailableUpgrades.Count);
                string randomUpgradeKey = upgradeKeys[index];
                AbilityUpgrades selectedUpgradeData = allAvailableUpgrades[randomUpgradeKey];

                if (!newUpgradeList.ContainsKey(randomUpgradeKey))
                {
                    newUpgradeList.Add(randomUpgradeKey, selectedUpgradeData);
                }
                else
                {
                    while (newUpgradeList.ContainsKey(randomUpgradeKey))
                    {
                        index = BaseUtilityMethods.GenerateRandomIndex(allAvailableUpgrades.Count);
                        randomUpgradeKey = upgradeKeys[index];
                    }

                    selectedUpgradeData = allAvailableUpgrades[randomUpgradeKey];
                    newUpgradeList.Add(randomUpgradeKey, selectedUpgradeData);
                }

                x++;
            }

            return newUpgradeList;
        }        
    }    
}