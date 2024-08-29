using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCardGenerator : MonoBehaviour
{
    [SerializeField] AbilityLibraryData abilityLibraryData;
    private GenerateAbilityCards generateAbilityCards;
    private GenerateUpgradeCards generateUpgradeCards;
    public event Action<List<AbilityLibraryData.AbilityStats>, Dictionary<string, AbilityUpgrades>> OnAbilitiesGenerated;

    private void Awake()
    {
        generateAbilityCards = GetComponent<GenerateAbilityCards>();
        generateUpgradeCards = GetComponent<GenerateUpgradeCards>();
    }

    public void BeginGeneration()
    {
        List<AbilityLibraryData.AbilityStats> newAbilities = generateAbilityCards.StartGeneratingAbilityCards(abilityLibraryData);
        Dictionary<string, AbilityUpgrades> newUpgrades = generateUpgradeCards.StartGeneratingUpgradeCards(abilityLibraryData);

        if (newAbilities != null && newUpgrades != null)
        {
            OnAbilitiesGenerated?.Invoke(newAbilities, newUpgrades);
        }        
    }
}
