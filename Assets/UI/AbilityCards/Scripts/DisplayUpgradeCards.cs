using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

[RequireComponent(typeof(DisplayCardManager))]
public class DisplayUpgradeCards : MonoBehaviour
{
    private List<GameObject> shuffledList;
    private Dictionary<GameObject, UpgradeTypesDatabase> upgradeCardRelationship = new Dictionary<GameObject, UpgradeTypesDatabase>();
    public Dictionary<GameObject, UpgradeTypesDatabase> UpgradeCardRelationship => upgradeCardRelationship;

    public void ProcessUpgradeDisplay(List<GameObject> shuffledList, int numToDisplay, List<UpgradeTypesDatabase> upgradeDatabase)
    {
        Logger.Log("Starting to display upgrade cards", this);
        this.shuffledList = shuffledList;
        DisplayUpgrades(numToDisplay, upgradeDatabase);
    }

    private void DisplayUpgrades(int numToDisplay, List<UpgradeTypesDatabase> upgradeDatabase)
    {
        upgradeCardRelationship.Clear();
        int cardIndex = CardUtilityMethods.GetCardIndex();

        if (numToDisplay > 0)
        {
            int x = 0;

            while (x < numToDisplay)
            {
                TextMeshProUGUI[] textElements = shuffledList[cardIndex].GetComponentsInChildren<TextMeshProUGUI>();

                UpgradeTypesDatabase upgradeDictionary = upgradeDatabase[x];

                // Format text to display
                string abilityName = BaseUtilityMethods.InsertSpaceBeforeCapitalLetters(upgradeDictionary.First().Key.ToString());
                string upgradeType = BaseUtilityMethods.InsertSpaceBeforeCapitalLetters(upgradeDictionary.First().Value.First().Key.ToString());
                string upgradeDescription = upgradeDictionary.First().Value.First().Value.First().description;
                string upgradeLevel = upgradeDictionary.First().Value.First().Value.Peek().level.ToString();

                // Update UI text
                textElements[0].text = $"Upgrade to\nlevel {upgradeLevel}!!";
                textElements[1].text = $"{abilityName}\n{upgradeType}";
                textElements[2].text = upgradeDescription;

                upgradeCardRelationship.Add(shuffledList[cardIndex], upgradeDictionary);

                x++;
                cardIndex++;
                CardUtilityMethods.SetCardIndex(cardIndex);
            }
        }

        // NOTE: TODO: Below is unncessary once I have the logic to hide UI panels implemented.
        if (cardIndex <= 2)
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
