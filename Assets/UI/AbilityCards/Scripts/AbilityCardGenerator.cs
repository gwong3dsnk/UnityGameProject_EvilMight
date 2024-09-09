using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

[RequireComponent(typeof(GenerateAbilityCards))]
[RequireComponent(typeof(GenerateUpgradeCards))]
public class AbilityCardGenerator : MonoBehaviour
{
    [SerializeField] AbilityLibraryData abilityLibraryData;
    [SerializeField] UpgradeLibraryData upgradeLibraryData;
    private GenerateAbilityCards generateAbilityCards;
    private GenerateUpgradeCards generateUpgradeCards;
    public event Action<List<AbilityLibraryData.AbilityStats>, List<UpgradeTypesDatabase>> OnAbilitiesGenerated;

    private void Awake()
    {
        generateAbilityCards = GetComponent<GenerateAbilityCards>();
        generateUpgradeCards = GetComponent<GenerateUpgradeCards>();
        Logger.Log(abilityLibraryData == null ? "Missing reference to AbilityLibraryData" : "AbilityLibraryData reference found.");
        Logger.Log(upgradeLibraryData == null ? "Missing reference to upgradeLibraryData" : "upgradeLibraryData reference found.");
        Logger.Log(generateAbilityCards == null ? "Missing local component GenerateAbilityCards" : "GenerateAbilityCards component found.");
        Logger.Log(generateAbilityCards == null ? "Missing local component GenerateUpgradeCards" : "GenerateUpgradeCards component found.");
    }

    public void BeginGeneration()
    {
        List<AbilityLibraryData.AbilityStats> newAbilities = generateAbilityCards.StartGeneratingAbilityCards(abilityLibraryData);
        // Generate newUpgrades populated with up to 3 upgrades with available levelData
        List<UpgradeTypesDatabase> newUpgrades = generateUpgradeCards.StartGeneratingUpgradeCards(upgradeLibraryData);

        if (newAbilities != null && newUpgrades != null)
        {
            OnAbilitiesGenerated?.Invoke(newAbilities, newUpgrades);
        }        
    }
}
