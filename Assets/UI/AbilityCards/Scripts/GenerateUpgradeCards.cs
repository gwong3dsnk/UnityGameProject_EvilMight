using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateUpgradeCards : MonoBehaviour
{
    Dictionary<string, AbilityUpgrades> abilityUpgradeData = new Dictionary<string, AbilityUpgrades>();      

    public Dictionary<string, AbilityUpgrades> StartGeneratingUpgradeCards(AbilityLibraryData abilityLibraryData)
    {
        CreateListOfAvailableUpgrades(abilityLibraryData);
        Dictionary<string, AbilityUpgrades> newUpgrades = CreateUpgradesList();

        return newUpgrades;
    }

    private void CreateListOfAvailableUpgrades(AbilityLibraryData abilityLibraryData)
    {
        abilityUpgradeData.Clear();

        foreach (AbilityLibraryData.AbilityStats data in abilityLibraryData.abilityStatsArray)
        {
            foreach (PlayerAbilities activeAbility in PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities)
            {
                if (PlayerAbilitiesManager.AbilityManagerInstance.ActiveUpgrades.Count > 0)
                {
                    foreach (AbilityUpgrades upgrade in data.abilityUpgrades)
                    {
                        foreach (KeyValuePair<string, AbilityUpgrades> item in PlayerAbilitiesManager.AbilityManagerInstance.ActiveUpgrades)
                        {
                            Debug.Log("Some upgrades have been unlocked.  Need to compare locked vs unlocked and add only locked ones.");
                        }
                    }
                }
                else
                {
                    string compressedName = data.abilityName.Replace(" ", "");
                    if (activeAbility.name.Contains(compressedName))
                    {
                        for (int i = 0; i < data.abilityUpgrades.Length; i++)
                        {
                            abilityUpgradeData.Add($"{data.abilityName}_idx{i}", data.abilityUpgrades[i]);
                        }
                    }
                }
            }
        }
    }

    private Dictionary<string, AbilityUpgrades> CreateUpgradesList()
    {
        Dictionary<string, AbilityUpgrades> newUpgradeList = new Dictionary<string, AbilityUpgrades>();
        List<string> upgradeKeys = new List<string>();

        if (abilityUpgradeData.Count <= 3)
        {
            return abilityUpgradeData;
        }
        else
        {
            int x = 0;

            while (x < 3)
            {
                foreach (KeyValuePair<string, AbilityUpgrades> upgradePair in abilityUpgradeData)
                {
                    upgradeKeys.Add(upgradePair.Key);
                }

                int index = BaseUtilityMethods.GenerateRandomIndex(abilityUpgradeData.Count);
                string randomUpgradeKey = upgradeKeys[index];
                AbilityUpgrades selectedUpgradeData = abilityUpgradeData[randomUpgradeKey];

                if (!newUpgradeList.ContainsKey(randomUpgradeKey))
                {
                    newUpgradeList.Add(randomUpgradeKey, selectedUpgradeData);
                }
                else
                {
                    while (newUpgradeList.ContainsKey(randomUpgradeKey))
                    {
                        index = BaseUtilityMethods.GenerateRandomIndex(abilityUpgradeData.Count);
                        randomUpgradeKey = upgradeKeys[index];
                    }

                    selectedUpgradeData = abilityUpgradeData[randomUpgradeKey];
                    newUpgradeList.Add(randomUpgradeKey, selectedUpgradeData);
                }

                x++;
            }

            return newUpgradeList;
        }        
    }    
}