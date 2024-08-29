using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCardGenerator : MonoBehaviour
{
    [SerializeField] PlayerAbilitiesManager abilityManager;
    [SerializeField] AbilityLibraryData abilityLibraryData;
    List<AbilityLibraryData.AbilityStats> abilityData = new List<AbilityLibraryData.AbilityStats>();
    Dictionary<string, AbilityUpgrades> abilityUpgradeData = new Dictionary<string, AbilityUpgrades>();      
    public event Action<List<AbilityLibraryData.AbilityStats>, Dictionary<string, AbilityUpgrades>> OnAbilitiesGenerated;

    public void GenerateAbilityCards()
    {
        CreateListOfAvailableAbilities();
        CreateListOfAvailableUpgrades();
        List<AbilityLibraryData.AbilityStats> newAbilities = CreateNewAbilityList();
        Dictionary<string, AbilityUpgrades> newUpgrades = CreateUpgradesList();

        // Debug.Log("Contents of abilityUpgradeData");
        // foreach (KeyValuePair<string, AbilityUpgrades> kvp in abilityUpgradeData)
        // {
        //     Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value.upgradeType}");
        // }

        if (newAbilities != null && newUpgrades != null)
        {
            OnAbilitiesGenerated?.Invoke(newAbilities, newUpgrades);
        }
    }

    private List<AbilityLibraryData.AbilityStats> CreateNewAbilityList()
    {
        List<AbilityLibraryData.AbilityStats> newAbilityList = new List<AbilityLibraryData.AbilityStats>();

        if (abilityData.Count <= 3)
        {
            return abilityData;
        }
        else
        {
            int x = 0;

            while (x < 3)
            {
                int index = BaseUtilityMethods.GenerateRandomIndex(abilityData.Count);

                if (!newAbilityList.Contains(abilityData[index]))
                {
                    newAbilityList.Add(abilityData[index]);
                }
                else
                {
                    while (newAbilityList.Contains(abilityData[index]))
                    {
                        index = BaseUtilityMethods.GenerateRandomIndex(abilityData.Count);
                    }

                    newAbilityList.Add(abilityData[index]);
                }

                x++;
            }

            return newAbilityList;
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

    private void CreateListOfAvailableAbilities()
    {
        abilityData.Clear();

        foreach (AbilityLibraryData.AbilityStats data in abilityLibraryData.abilityStatsArray)
        {
            foreach (PlayerAbilities activeAbility in abilityManager.ActiveAbilities)
            {
                if (!activeAbility.name.Contains(data.playerAbilities.name))
                {
                    // Debug.Log($"Ability - {data.playerAbilities.name} - not unlocked.  Adding to abilityData.");
                    abilityData.Add(data);
                }
            }
        }
    }

    private void CreateListOfAvailableUpgrades()
    {
        abilityUpgradeData.Clear();

        foreach (AbilityLibraryData.AbilityStats data in abilityLibraryData.abilityStatsArray)
        {
            foreach (PlayerAbilities activeAbility in abilityManager.ActiveAbilities)
            {
                if (abilityManager.ActiveUpgrades.Count > 0)
                {
                    foreach (AbilityUpgrades upgrade in data.abilityUpgrades)
                    {
                        foreach (KeyValuePair<string, AbilityUpgrades> item in abilityManager.ActiveUpgrades)
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
}
