using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilityDataDictionary = System.Collections.Generic.Dictionary<AbilityUpgradeType, AbilityLibraryData.AbilityStats>;

public class AbilityCardGenerator : MonoBehaviour
{
    [SerializeField] PlayerAbilitiesManager abilityManager;
    [SerializeField] AbilityLibraryData abilityLibraryData;
    List<AbilityLibraryData.AbilityStats> abilityData = new List<AbilityLibraryData.AbilityStats>();
    Dictionary<string, AbilityDataDictionary> abilityUpgradeData = new Dictionary<string, AbilityDataDictionary>();      
    public event Action<AbilityLibraryData.AbilityStats, List<Dictionary<string, AbilityDataDictionary>>> OnAbilitiesGenerated;

    public void GenerateAbilityCards()
    {
        CreateAbilityLists();
        AbilityLibraryData.AbilityStats newAbility = ChooseRandomAbility();
        List<Dictionary<string, AbilityDataDictionary>> newUpgrades = ChooseRandomUpgrades();

        if (newAbility != null && newUpgrades != null)
        {
            // Debug.Log($"AbilityToDisplay - {newAbility.abilityName}");
            OnAbilitiesGenerated?.Invoke(newAbility, newUpgrades);
        }
    }

    AbilityLibraryData.AbilityStats ChooseRandomAbility()
    {
        int index = UtilityMethods.GenerateRandomIndex(abilityData.Count);
        
        // TODO: Put in logic to handle when the player has obtained all abilities.  Only Upgrades should be chosen.
        if (abilityData.Count > 0)
        {
            return abilityData[index];
        }
        else
        {
            Debug.Log("Player has unlocked all abilities.  Replacing with upgrade");
            return null;
            // Check if there are still ability upgrades available.
            // If there are, choose one and replace this card with the upgrade.
        }
    }

    List<Dictionary<string, AbilityDataDictionary>> ChooseRandomUpgrades()
    {
        List<Dictionary<string, AbilityDataDictionary>> chosenUpgrades = new List<Dictionary<string, AbilityDataDictionary>>();
        List<string> upgradeKeys = new List<string>();

        if (abilityUpgradeData.Count > 0)
        {
            int x = 0;
            // Continue loop until we have obtained 2 random ability upgrades
            while (x < 2)
            {
                foreach (KeyValuePair<string, AbilityDataDictionary> upgradePair in abilityUpgradeData)
                {
                    upgradeKeys.Add(upgradePair.Key);
                }

                int index = UtilityMethods.GenerateRandomIndex(abilityUpgradeData.Count);

                string randomUpgradeKey = upgradeKeys[index];
                AbilityDataDictionary selectedDataDict = abilityUpgradeData[randomUpgradeKey];
                Dictionary<string, AbilityDataDictionary> completeUpgradeDict = new Dictionary<string, AbilityDataDictionary> { { randomUpgradeKey, selectedDataDict } };
                chosenUpgrades.Add(completeUpgradeDict);

                x++;
            }

            return chosenUpgrades;

            // foreach (var item in chosenUpgrades)
            // {
            //     foreach (KeyValuePair<string, AbilityDataDictionary> data in item)
            //     {
            //         Debug.Log($"Chosen UPgrade Key - {data.Key}");
            //         foreach (KeyValuePair<AbilityUpgradeType, AbilityLibraryData.AbilityStats> kvp in data.Value)
            //         {
            //             Debug.Log($"Chosen UPgrade ValueKey - {kvp.Key}");
            //             Debug.Log($"Chosen UPgrade ValueValue - {kvp.Value}");
            //         }
            //     }
            // }

        }
        else if (abilityUpgradeData.Count == 1)
        {
            // TODO: IN this case, we have 1 upgrade remaining.  Choose it to display it.
            Debug.Log("Less than two upgrades remain");
            return null;
        }
        else
        {
            Debug.LogError("Something is wrong as there are no upgrades in abilityUpgradeData or all upgrades are unlocked", this);
            return null;
        }
    }

    void CreateAbilityLists()
    {
        // TODO: Convert this method to be called when the OnLevelUp event is invoked.  
        // So everytime level up, redo the array.
        abilityData.Clear();
        abilityUpgradeData.Clear();

        foreach (AbilityLibraryData.AbilityStats data in abilityLibraryData.abilityStatsArray)
        {
            CreateNewAbilityList(data);
            CreateAbilityUpgradeList(data);
        }
    }

    private void CreateNewAbilityList(AbilityLibraryData.AbilityStats data)
    {
        foreach (PlayerAbilities activeAbility in abilityManager.ActiveAbilities)
        {
            // Debug.Log($"ActiveAbility Name - {activeAbility.name}");
            // Debug.Log($"Data Name - {data.abilityName}");
            if (!activeAbility.name.Contains(data.abilityName))
            {
                abilityData.Add(data);
            }
        }
    }

    private void CreateAbilityUpgradeList(AbilityLibraryData.AbilityStats data)
    {
        if (abilityManager.ActiveUpgrades.Count > 0)
        {
            Debug.Log("ActiveUpgrades exist > 0, need to write logic to handle regeneration of dictionary");
            foreach (AbilityUpgrades upgrade in data.abilityUpgrades)
            {
                foreach (KeyValuePair<string, AbilityUpgrades> item in abilityManager.ActiveUpgrades)
                {
                    // Debug.Log($"Upgrade-UpgradeType - {upgrade.upgradeType}");
                    // Debug.Log($"item.Value.upgradeTyp - {item.Value.upgradeType}");
                    // Debug.Log($"data.abilityName - {data.abilityName}");
                    // Debug.Log($"item.Key - {item.Key}");
                    // if (upgrade.upgradeType != item.Value.upgradeType && data.abilityName != item.Key)
                    // {
                    //     AbilityDataDictionary dataDict = new AbilityDataDictionary { { upgrade.upgradeType, data } };
                    //     abilityUpgradeData.Add(data.abilityName, dataDict);
                    // }
                }
            }
        }
        else
        {
            // First generation of ability upgrade list on Game start
            for (int i = 0; i < data.abilityUpgrades.Length; i++)
            {
                AbilityDataDictionary dataDict = new AbilityDataDictionary { { data.abilityUpgrades[i].upgradeType, data } };
                abilityUpgradeData.Add($"{data.abilityName}_idx_{i}", dataDict);
            }
        }
    }
}
