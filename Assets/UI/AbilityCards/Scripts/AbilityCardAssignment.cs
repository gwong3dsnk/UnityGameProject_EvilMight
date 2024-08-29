using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AbilityCardGenerator))]
public class AbilityCardAssignment : MonoBehaviour
{
    [SerializeField] private GameObject[] cardPanels;
    private AbilityCardGenerator cardGenerator;
    private List<GameObject> shuffledList;
    private int cardIndex;
    private List<AbilityLibraryData.AbilityStats> generatedAbilities;
    private Dictionary<string, AbilityUpgrades> generatedUpgrades;
    private Dictionary<GameObject, AbilityLibraryData.AbilityStats> abilityCardRelationship = new Dictionary<GameObject, AbilityLibraryData.AbilityStats>();
    public Dictionary<GameObject, AbilityLibraryData.AbilityStats> AbilityCardRelationship => abilityCardRelationship;
    private Dictionary<GameObject, Dictionary<string, AbilityUpgrades>> upgradeCardRelationship = new Dictionary<GameObject, Dictionary<string, AbilityUpgrades>>();
    public Dictionary<GameObject, Dictionary<string, AbilityUpgrades>> UpgradeCardRelationship => upgradeCardRelationship;

    private void Awake()
    {
        cardGenerator = GetComponent<AbilityCardGenerator>();

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
        List<AbilityLibraryData.AbilityStats> chosenAbilities = SelectRandomAbilitiesToDisplay(finalNumOfAbilitiesToDisplay);
        DisplayAbilities(chosenAbilities);

        if (finalNumOfAbilitiesToDisplay < 3)
        {
            Dictionary<string, AbilityUpgrades> chosenUpgradeKeys = SelectRandomUpgradesToDisplay(finalNumOfUpgradesToDisplay);
            DisplayUpgrades(chosenUpgradeKeys);
        }
    }

    private List<AbilityLibraryData.AbilityStats> SelectRandomAbilitiesToDisplay(int numToSelect)
    {
        List<AbilityLibraryData.AbilityStats> randomlySelectedAbilities = new List<AbilityLibraryData.AbilityStats>();
        AbilityLibraryData.AbilityStats chosenAbility = GetRandomAbilityData();

        while (numToSelect > 0)
        {
            while (randomlySelectedAbilities.Contains(chosenAbility))
            {
                chosenAbility = GetRandomAbilityData();
            }

            randomlySelectedAbilities.Add(chosenAbility);
            numToSelect--;
        }

        return randomlySelectedAbilities;
    }

    private Dictionary<string, AbilityUpgrades> SelectRandomUpgradesToDisplay(int numToSelect)
    {
        List<string> allUpgradeKeys = CreateListOfUpgradeKeys();
        Dictionary<string, AbilityUpgrades> chosenUpgrades = new Dictionary<string, AbilityUpgrades>();
        string randomUpgradeKey = GetRandomUpgradeKey(allUpgradeKeys);

        while (numToSelect > 0)
        {
            while (chosenUpgrades.ContainsKey(randomUpgradeKey))
            {
                randomUpgradeKey = GetRandomUpgradeKey(allUpgradeKeys);
            }

            chosenUpgrades.Add(randomUpgradeKey, generatedUpgrades[randomUpgradeKey]);
            numToSelect--;
        }

        return chosenUpgrades;
    }

    private string GetRandomUpgradeKey(List<string> upgradeKeys)
    {
        int index = BaseUtilityMethods.GenerateRandomIndex(upgradeKeys.Count);
        return upgradeKeys[index];
    }

    private AbilityLibraryData.AbilityStats GetRandomAbilityData()
    {
        int randomIndex = BaseUtilityMethods.GenerateRandomIndex(generatedAbilities.Count);
        return generatedAbilities[randomIndex];
    }

    private List<string> CreateListOfUpgradeKeys()
    {
        List<string> upgradeKeys = new List<string>();

        foreach (KeyValuePair<string, AbilityUpgrades> kvp in generatedUpgrades)
        {
            upgradeKeys.Add(kvp.Key);
        }

        return upgradeKeys;
    }

    private void ShufflePanelCards()
    {
        shuffledList = BaseUtilityMethods.ShuffleList(cardPanels.ToList());
    }

    private void DisplayAbilities(List<AbilityLibraryData.AbilityStats> chosenAbilities)
    {
        cardIndex = 0;
        abilityCardRelationship.Clear();

        while (chosenAbilities.Count > cardIndex)
        {
            TextMeshProUGUI[] textElements = shuffledList[cardIndex].GetComponentsInChildren<TextMeshProUGUI>();
            textElements[0].text = "New Ability!!";
            textElements[1].text = chosenAbilities[cardIndex].abilityName;
            textElements[2].text = chosenAbilities[cardIndex].abilityDescription;

            abilityCardRelationship.Add(shuffledList[cardIndex], chosenAbilities[cardIndex]);

            cardIndex++;
        }
    }

    private void DisplayUpgrades(Dictionary<string, AbilityUpgrades> chosenUpgrades)
    {
        upgradeCardRelationship.Clear();
        List<KeyValuePair<string, AbilityUpgrades>> chosenUpgradeList = chosenUpgrades.ToList();
        int x = 0;

        while (chosenUpgrades.Count >= cardIndex)
        {
            TextMeshProUGUI[] textElements = shuffledList[cardIndex].GetComponentsInChildren<TextMeshProUGUI>();

            // Format text to display
            string upgradeType = BaseUtilityMethods.InsertSpaceBeforeCapitalLetters(chosenUpgradeList[x].Value.upgradeType.ToString());
            string abilityName = AbilityUtilityMethods.FormatAbilityName(chosenUpgradeList[x].Key);
            string upgradeDescription = chosenUpgradeList[x].Value.upgradeDescription;

            // Update UI text
            textElements[0].text = "New Upgrade!!";
            textElements[1].text = $"{abilityName}\n{upgradeType}";
            textElements[2].text = upgradeDescription;

            // Add new upgade-card relation
            Dictionary <string, AbilityUpgrades> upgradeToAdd = new Dictionary<string, AbilityUpgrades>();
            upgradeToAdd.Add(chosenUpgradeList[x].Key, chosenUpgradeList[x].Value);
            upgradeCardRelationship.Add(shuffledList[cardIndex], upgradeToAdd);

            x++;
            cardIndex++;
        }
    }    
}
