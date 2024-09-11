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
    private GenerateAbilityCards generateAbilityCards;
    private GenerateUpgradeCards generateUpgradeCards;
    public event Action<List<AbilityLibraryData.AbilityStats>, List<UpgradeTypesDatabase>> OnAbilitiesGenerated;

    private void Awake()
    {
        generateAbilityCards = GetComponent<GenerateAbilityCards>();
        generateUpgradeCards = GetComponent<GenerateUpgradeCards>();

        if (generateAbilityCards == null || generateUpgradeCards == null)
        {
            Logger.LogError("Missing either GenerateAbilityCards or GenerateUpgradeCards local component", this);
        }
    }

    public void BeginGeneration()
    {
        List<AbilityLibraryData.AbilityStats> newAbilities = generateAbilityCards.StartGeneratingAbilityCards();
        List<UpgradeTypesDatabase> newUpgrades = generateUpgradeCards.StartGeneratingUpgradeCards();

        if (newAbilities != null && newUpgrades != null)
        {
            OnAbilitiesGenerated?.Invoke(newAbilities, newUpgrades);
        }        
    }
}
