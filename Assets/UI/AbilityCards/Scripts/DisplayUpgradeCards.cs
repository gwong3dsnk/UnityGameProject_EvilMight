using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DisplayCardManager))]
public class DisplayUpgradeCards : MonoBehaviour
{
    private List<GameObject> shuffledList;
    private Dictionary<string, AbilityUpgrades> generatedUpgrades;
    private Dictionary<GameObject, Dictionary<string, AbilityUpgrades>> upgradeCardRelationship = new Dictionary<GameObject, Dictionary<string, AbilityUpgrades>>();
    public Dictionary<GameObject, Dictionary<string, AbilityUpgrades>> UpgradeCardRelationship => upgradeCardRelationship;
    private int cardIndex;

    public void ProcessUpgradeDisplay(List<GameObject> shuffledList, int finalNumOfUpgradesToDisplay, Dictionary<string, AbilityUpgrades> generatedUpgrades)
    {
        this.cardIndex = DisplayAbilityCards.cardIndex;
        this.shuffledList = shuffledList;
        this.generatedUpgrades = generatedUpgrades;
        Dictionary<string, AbilityUpgrades> chosenUpgradeKeys = SelectRandomUpgradesToDisplay(finalNumOfUpgradesToDisplay);
        DisplayUpgrades(chosenUpgradeKeys);
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

            chosenUpgrades.Add(randomUpgradeKey, this.generatedUpgrades[randomUpgradeKey]);
            numToSelect--;
        }

        return chosenUpgrades;
    }

    private string GetRandomUpgradeKey(List<string> upgradeKeys)
    {
        int index = BaseUtilityMethods.GenerateRandomIndex(upgradeKeys.Count);
        return upgradeKeys[index];
    }

    private List<string> CreateListOfUpgradeKeys()
    {
        List<string> upgradeKeys = new List<string>();

        foreach (KeyValuePair<string, AbilityUpgrades> kvp in this.generatedUpgrades)
        {
            upgradeKeys.Add(kvp.Key);
        }

        return upgradeKeys;
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
