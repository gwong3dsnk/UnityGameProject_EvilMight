using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AbilityCardGenerator))]
[RequireComponent(typeof(DisplayAbilityCards))]
[RequireComponent(typeof(DisplayUpgradeCards))]
public class DisplayCardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cardPanels;
    private AbilityCardGenerator cardGenerator;
    private List<GameObject> shuffledList;
    private List<AbilityLibraryData.AbilityStats> generatedAbilities;
    private Dictionary<string, AbilityUpgrades> generatedUpgrades;
    private DisplayAbilityCards displayAbilityCards;
    private DisplayUpgradeCards displayUpgradeCards;

    private void Awake()
    {
        cardGenerator = GetComponent<AbilityCardGenerator>();
        displayAbilityCards = GetComponent<DisplayAbilityCards>();
        displayUpgradeCards = GetComponent<DisplayUpgradeCards>();

        if (cardGenerator == null)
        {
            Debug.LogError("Missing reference to AbilityCardGenerator component", this);
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

    private void CalculateTotalAbilitiesToDisplay(List<AbilityLibraryData.AbilityStats> abilities, Dictionary<string, AbilityUpgrades> upgrades)
    {
        generatedAbilities = abilities; // Should never number > 3
        generatedUpgrades = upgrades;

        // Determine how many new abilities to display based on how many upgradesToDisplay are available.
        int abilityCountBasedOnUpgradeCount = upgrades.Count >= 2 ? 1 : (upgrades.Count == 1 ? 2 : 3);
        int finalNumOfAbilitiesToDisplay = 0;
        int finalNumOfUpgradesToDisplay = 0;

        if (abilities.Count == 0 && upgrades.Count == 0)
        {
            Debug.Log("All abilities and upgrades have been unlocked!");
            // TODO: If everything is unlocked, don't levelup anymore and don't freeze time/show cards anymore.
            return;
        }

        if (abilities.Count == 0 && upgrades.Count > 0)
        {
            finalNumOfAbilitiesToDisplay = 0;
            finalNumOfUpgradesToDisplay = upgrades.Count > 3 ? 3 : upgrades.Count;
        }
        else if (abilities.Count > 0 && upgrades.Count == 0)
        {
            finalNumOfAbilitiesToDisplay = abilities.Count > 3 ? 3 : upgrades.Count;
            finalNumOfUpgradesToDisplay = 0;
        }
        else if (abilityCountBasedOnUpgradeCount > abilities.Count)
        {
            finalNumOfAbilitiesToDisplay = abilities.Count;
            finalNumOfUpgradesToDisplay = 3 - abilities.Count;
        }
        else if (abilityCountBasedOnUpgradeCount <= abilities.Count)
        {
            finalNumOfAbilitiesToDisplay = abilityCountBasedOnUpgradeCount;
            finalNumOfUpgradesToDisplay = 3 - abilityCountBasedOnUpgradeCount;
        }
        else
        {
            Debug.LogWarning("Strange occurrence of ability/upgrade count.", this);
        }

        ProcessDisplayAbilitiesAndUpgrades(finalNumOfAbilitiesToDisplay, finalNumOfUpgradesToDisplay);
    }

    private void ProcessDisplayAbilitiesAndUpgrades(int finalNumOfAbilitiesToDisplay, int finalNumOfUpgradesToDisplay)
    {
        ShufflePanelCards();
        displayAbilityCards.ProcessAbilityDisplay(shuffledList, finalNumOfAbilitiesToDisplay, generatedAbilities);

        if (finalNumOfAbilitiesToDisplay < 3)
        {
            displayUpgradeCards.ProcessUpgradeDisplay(shuffledList, finalNumOfUpgradesToDisplay, generatedUpgrades);
        }
    }

    private void ShufflePanelCards()
    {
        shuffledList = BaseUtilityMethods.ShuffleList(cardPanels.ToList());
    }      
}
