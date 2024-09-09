using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

[RequireComponent(typeof(DisplayCardManager))]
public class DisplayUpgradeCards : MonoBehaviour
{
    private List<GameObject> shuffledList;
    // private UpgradeTypesDatabase generatedUpgrades;
    private Dictionary<GameObject, UpgradeTypesDatabase> upgradeCardRelationship = new Dictionary<GameObject, UpgradeTypesDatabase>();
    public Dictionary<GameObject, UpgradeTypesDatabase> UpgradeCardRelationship => upgradeCardRelationship;
    private int cardIndex;

    public void ProcessUpgradeDisplay(List<GameObject> shuffledList, int numToDisplay, List<UpgradeTypesDatabase> upgradeDatabase)
    {
        // this.cardIndex = DisplayAbilityCards.cardIndex;
        this.cardIndex = CardUtilityMethods.GetCardIndex();
        this.shuffledList = shuffledList;
        // this.generatedUpgrades = generatedUpgrades;
        // UpgradeTypesDatabase chosenUpgradeKeys = SelectRandomUpgradesToDisplay(numToDisplay);
        // DisplayUpgrades(chosenUpgradeKeys);
        DisplayUpgrades(numToDisplay, upgradeDatabase);
    }

    // private UpgradeTypesDatabase SelectRandomUpgradesToDisplay(int numToSelect)
    // {
    //     UpgradeTypesDatabase chosenUpgrades = new UpgradeTypesDatabase();

    //     List<Dictionary<AbilityNames, UpgradeTypes>> allUpgradeOptions = CreateListOfUpgradeKeys();
    //     // Dictionary<AbilityNames, UpgradeTypes> randomUpgradeOption = GetRandomUpgradeOption(allUpgradeOptions);

    //     while (numToSelect > 0)
    //     {
    //         while (chosenUpgrades.ContainsKey(randomUpgradeOption))
    //         {
    //             randomUpgradeOption = GetRandomUpgradeOption(allUpgradeOptions);
    //         }

    //         // chosenUpgrades.Add(randomUpgradeKey, this.generatedUpgrades[randomUpgradeKey]);
    //         numToSelect--;
    //     }

    //     return chosenUpgrades;
    // }

    // private List<Dictionary<AbilityNames, UpgradeTypes>> CreateListOfUpgradeKeys()
    // {
    //     List<Dictionary<AbilityNames, UpgradeTypes>> upgradeOptions = new List<Dictionary<AbilityNames, UpgradeTypes>>();

    //     foreach (var kvp in this.generatedUpgrades)
    //     {
    //         foreach (var type in kvp.Value)
    //         {
    //             Dictionary<AbilityNames, UpgradeTypes> option = new Dictionary<AbilityNames, UpgradeTypes> { { kvp.Key, type.Key }};
    //             upgradeOptions.Add(option);
    //         }
    //     }

    //     return upgradeOptions;
    // }

    // private Dictionary<AbilityNames, UpgradeTypes> GetRandomUpgradeOption(List<Dictionary<AbilityNames, UpgradeTypes>> allUpgradeOptions)
    // {
    //     int index = GeneralUtilityMethods.GenerateRandomIndex(allUpgradeOptions.Count);
    //     return allUpgradeOptions[index];
    // }    

    private void DisplayUpgrades(int numToDisplay, List<UpgradeTypesDatabase> upgradeDatabase)
    {
        upgradeCardRelationship.Clear();
        // int numAvailableUpgrades = CardUtilityMethods.GetNumValidLevelQueues(this.generatedUpgrades);
        // bool isEverythingUnlocked = false;
        // int j = 0;

        // if (cardIndex == 0 && numAvailableUpgrades < 3)
        // {
        //     Logger.Log("No abilities displayed, less than 3 available upgrades to display", this);
        //     switch (numAvailableUpgrades)
        //     {
        //         case 2:
        //             Logger.Log("Only 2 upgrades available");
        //             j = numAvailableUpgrades - 1;   
        //             break;
        //         case 1:
        //             Logger.Log($"Only 1 upgrade available");
        //             j = numAvailableUpgrades;
        //             break;         
        //         case 0:       
        //             Logger.LogError("No abilities and No upgrades are available to be displayed.", this);
        //             isEverythingUnlocked = true;   
        //             break;                       
        //     }
        // }

            // Call method that will remove the third ability card UI and shift the position of the remaining two.
            // Start by disabling the last ability card UI panel
            // Get full screen width and width of the remaining panel-images
            // Split the screen in thirds and position each card at the third intervals.

        if (numToDisplay > 0)
        {
            int x = 0;

            while (x < numToDisplay)
            {
                TextMeshProUGUI[] textElements = shuffledList[cardIndex].GetComponentsInChildren<TextMeshProUGUI>();

                UpgradeTypesDatabase upgradeDictionary = upgradeDatabase[x];

                // Format text to display
                string abilityName = AbilityUtilityMethods.FormatAbilityName(upgradeDictionary.First().Key.ToString());
                string upgradeType = BaseUtilityMethods.InsertSpaceBeforeCapitalLetters(upgradeDictionary.First().Value.First().Key.ToString());
                string upgradeDescription = upgradeDictionary.First().Value.First().Value.First().description;
                string upgradeLevel = upgradeDictionary.First().Value.First().Value.First().level.ToString();
                //int newValue = upgradeDictionary.First().Value.First().Value.First().newValue;

                // Update UI text
                textElements[0].text = $"Upgrade to level {upgradeLevel}!!";
                textElements[1].text = $"{abilityName}\n{upgradeType}";
                textElements[2].text = upgradeDescription;

                // Add new upgade-card relation
                // Dictionary <string, AbilityUpgrades> upgradeToAdd = new Dictionary<string, AbilityUpgrades>();
                // upgradeToAdd.Add(chosenUpgradeList[x].Key, chosenUpgradeList[x].Value);
                upgradeCardRelationship.Add(shuffledList[cardIndex], upgradeDictionary);

                x++;
                cardIndex++;
            }
        }

        // NOTE: Below is unncessary once I have the logic to hide UI panels implemented.
        if (cardIndex < 2)
        {
            int j = 0;
            while (j < cardIndex)
            {
                TextMeshProUGUI[] textElements = shuffledList[cardIndex].GetComponentsInChildren<TextMeshProUGUI>();

                textElements[0].text = "All Done!!";
                textElements[1].text = "Everything is\nUnlocked!!";
                textElements[2].text = "Congrats you've unlocked all abilities and upgrades!";  

                j++;                      
            }
        }
    } 
}   
