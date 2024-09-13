using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

[RequireComponent(typeof(AbilityCardGenerator))]
[RequireComponent(typeof(DisplayAbilityCards))]
[RequireComponent(typeof(DisplayUpgradeCards))]
public class DisplayCardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cardUIPanels;
    private AbilityCardGenerator cardGenerator;
    private List<GameObject> shuffledList;
    private List<AbilityLibraryData.AbilityStats> abilities;
    private List<UpgradeTypesDatabase> upgrades;
    private DisplayAbilityCards displayAbilityCards;
    private DisplayUpgradeCards displayUpgradeCards;

    private void Awake()
    {
        cardGenerator = GetComponent<AbilityCardGenerator>();
        displayAbilityCards = GetComponent<DisplayAbilityCards>();
        displayUpgradeCards = GetComponent<DisplayUpgradeCards>();

        if (cardGenerator == null || displayAbilityCards == null || displayUpgradeCards == null)
        {
            Logger.LogError("Missing local component of AbilityCardGenerator, DisplayAbilitytCards or DisplayUpgradeCards", this);
        }
    }

    private void OnEnable() 
    {
        cardGenerator.OnAbilitiesGenerated += CalculateTotalAbilitiesToDisplay;
    }

    private void OnDisable()
    {
        cardGenerator.OnAbilitiesGenerated -= CalculateTotalAbilitiesToDisplay;
    }

    private void CalculateTotalAbilitiesToDisplay(List<AbilityLibraryData.AbilityStats> abilities, List<UpgradeTypesDatabase> upgrades)
    {
        Logger.Log("Starting to calculate total abilities to display in DisplayCardManager.\nDisplaying Content...", this);
        this.abilities = abilities;
        foreach (var item in this.abilities) // log
        {
            Logger.Log($"Ability Contents - {item.abilityName}", this);
        }
        this.upgrades = upgrades;
        foreach (var item in this.upgrades) // log
        {
            Logger.Log($"Upgrade Contents - {item.First().Value.First().Key}", this);
        }        
        int finalNumOfAbilitiesToDisplay = 0;
        int finalNumOfUpgradesToDisplay = 0;

        // Determine how many new abilities to display based on how many upgrades are available.
        int abilityCountBasedOnUpgradeCount = this.upgrades.Count >= 2 ? 1 : (this.upgrades.Count == 1 ? 2 : 3);

        if (this.abilities.Count == 0 && this.upgrades.Count == 0)
        {
            Logger.Log("All abilities and upgrades have been unlocked!");
            // TODO: If everything is unlocked, don't levelup anymore and don't freeze time/show cards anymore.
            return;
        }

        if (this.abilities.Count == 0 && this.upgrades.Count > 0)
        {
            Logger.Log($"0 abilities detected, {this.upgrades.Count} upgrade(s) detected.");
            finalNumOfAbilitiesToDisplay = 0;
            finalNumOfUpgradesToDisplay = this.upgrades.Count > 3 ? 3 : this.upgrades.Count;
        }
        else if (this.abilities.Count > 0 && this.upgrades.Count == 0)
        {
            Logger.Log($"{this.abilities.Count} abilities detected, 0 upgrades detected.");
            finalNumOfAbilitiesToDisplay = this.abilities.Count > 3 ? 3 : this.upgrades.Count;
            finalNumOfUpgradesToDisplay = 0;
        }
        else if (abilityCountBasedOnUpgradeCount > this.abilities.Count)
        {
            Logger.Log($"abilityCountBasedOnUpgradeCount - {abilityCountBasedOnUpgradeCount} > abilities.Count - {this.abilities.Count}");
            finalNumOfAbilitiesToDisplay = this.abilities.Count;
            finalNumOfUpgradesToDisplay = 3 - this.abilities.Count;
        }
        else if (abilityCountBasedOnUpgradeCount <= this.abilities.Count)
        {
            Logger.Log($"abilityCountBasedOnUpgradeCount - {abilityCountBasedOnUpgradeCount} <= abilities.Count - {this.abilities.Count}");
            finalNumOfAbilitiesToDisplay = abilityCountBasedOnUpgradeCount;
            finalNumOfUpgradesToDisplay = 3 - abilityCountBasedOnUpgradeCount;
        }
        else
        {
            Logger.LogWarning("Strange occurrence of ability/upgrade count.", this);
        }

        ProcessDisplayAbilitiesAndUpgrades(finalNumOfAbilitiesToDisplay, finalNumOfUpgradesToDisplay);
    }

    private void ProcessDisplayAbilitiesAndUpgrades(int finalNumOfAbilitiesToDisplay, int finalNumOfUpgradesToDisplay)
    {
        // Shuffle the array of the three UI Panels
        shuffledList = GeneralUtilityMethods.ShuffleList(cardUIPanels.ToList());

        displayAbilityCards.ProcessAbilityDisplay(shuffledList, finalNumOfAbilitiesToDisplay, this.abilities);

        if (finalNumOfAbilitiesToDisplay < 3)
        {
            displayUpgradeCards.ProcessUpgradeDisplay(shuffledList, finalNumOfUpgradesToDisplay, this.upgrades);
        }
    }
}
