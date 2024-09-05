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
            if (PlayerAbilitiesManager.AbilityManagerInstance.ActiveUpgrades.Count > 0)
            {
                for (int i = 0; i < data.abilityUpgrades.Length; i++)
                {
                    bool isFound = false;

                    foreach (KeyValuePair<string, AbilityUpgrades> kvp in PlayerAbilitiesManager.AbilityManagerInstance.ActiveUpgrades)
                    {
                        if (kvp.Key.Contains(data.abilityName.ToString()) && data.abilityUpgrades[i].upgradeType != kvp.Value.upgradeType)
                        {
                            isFound = true;
                            break;
                        }
                    }

                    if (!isFound)
                    {
                        if (!abilityUpgradeData.ContainsKey($"{data.abilityName}_idx{i}"))
                        {
                            abilityUpgradeData.Add($"{data.abilityName}_idx{i}", data.abilityUpgrades[i]);
                        }
                    }
                }

                foreach (var item in abilityUpgradeData)
                {
                    Debug.Log("Contents of AbilityUpgradeData after Generation");
                    Debug.Log(item.Key);
                    Debug.Log(item.Value.upgradeType);
                }
            }
            else
            {
                foreach (PlayerAbilities activeAbility in PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities)
                {
                    if (activeAbility.name.Contains(data.abilityName.ToString()))
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