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
        Logger.Log("------------------------------------------------", this);
        Logger.Log("Starting to DISPLAY UPGRADE cards", this);
        this.shuffledList = shuffledList;
        upgradeCardRelationship.Clear();
        if (numToDisplay > 0)
        {
            DisplayUpgrades(numToDisplay, upgradeDatabase);
        }
    }

    private void DisplayUpgrades(int numToDisplay, List<UpgradeTypesDatabase> upgradeDatabase)
    {
        int cardIndex = CardUtilityMethods.GetCardIndex();
        Logger.Log($"Current CardIndex Value - {cardIndex}", this);

        if (numToDisplay > 0)
        {
            int x = 0;

            Logger.Log($"Starting while loop to display upgrades with x [{x}] and numToDisplay [{numToDisplay}]", this);
            while (x < numToDisplay)
            {
                Logger.Log($"LOOPING, x [{x}], numToDisplay [{numToDisplay}]", this);
                Logger.Log($"Card Panel to Edit - {this.shuffledList[cardIndex].name}", this);
                TextMeshProUGUI[] textElements = shuffledList[cardIndex].GetComponentsInChildren<TextMeshProUGUI>();

                UpgradeTypesDatabase upgradeDictionary = upgradeDatabase[x];
                Logger.Log($"Ability to Assign to Card -[{upgradeDictionary.First().Key}], Upgrade - [{upgradeDictionary.First().Value.First().Key}]");

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

            Logger.Log($"While loop ended.  Final CardIndex Value - {CardUtilityMethods.GetCardIndex()}", this);
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
