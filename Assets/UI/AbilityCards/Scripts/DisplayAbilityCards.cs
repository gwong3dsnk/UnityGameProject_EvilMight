using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DisplayCardManager))]
public class DisplayAbilityCards : MonoBehaviour
{
    private List<GameObject> shuffledList;
    private List<AbilityLibraryData.AbilityStats> generatedAbilities;
    private Dictionary<GameObject, AbilityLibraryData.AbilityStats> abilityCardRelationship = new Dictionary<GameObject, AbilityLibraryData.AbilityStats>();
    public Dictionary<GameObject, AbilityLibraryData.AbilityStats> AbilityCardRelationship => abilityCardRelationship;

    public void ProcessAbilityDisplay(List<GameObject> shuffledList, int numToDisplay, List<AbilityLibraryData.AbilityStats> generatedAbilities)
    {
        Logger.Log("Starting to display ability card", this);
        CardUtilityMethods.SetCardIndex(0);
        abilityCardRelationship.Clear();
        this.shuffledList = shuffledList;
        this.generatedAbilities = generatedAbilities;
        
        if (numToDisplay > 0)
        {
            List<AbilityLibraryData.AbilityStats> chosenAbilities = SelectRandomAbilitiesToDisplay(numToDisplay);
            Logger.Log("Starting to log chosen ability content.", this);
            foreach (var item in chosenAbilities) // log
            {
                Logger.Log($"ChosenAbilities Content - {item.abilityName}");
            }
            Logger.Log("End logging chosen ability content.", this);
            DisplayAbilities(chosenAbilities);
        }
        else
        {
            Logger.Log("All abilities have been unlocked.  No new abilities to display.", this);
        }
    }

    private List<AbilityLibraryData.AbilityStats> SelectRandomAbilitiesToDisplay(int numToSelect)
    {
        List<AbilityLibraryData.AbilityStats> randomlySelectedAbilities = new List<AbilityLibraryData.AbilityStats>();
        AbilityLibraryData.AbilityStats chosenAbility;

        chosenAbility = this.generatedAbilities.Count > 0 ? GetRandomAbilityData() : null;

        if (chosenAbility != null)
        {
            while (numToSelect > 0)
            {
                while (randomlySelectedAbilities.Contains(chosenAbility))
                {
                    chosenAbility = GetRandomAbilityData();
                }

                randomlySelectedAbilities.Add(chosenAbility);
                numToSelect--;
            }
        }
        else
        {
            randomlySelectedAbilities.Clear();
        }

        return randomlySelectedAbilities;
    }

    private AbilityLibraryData.AbilityStats GetRandomAbilityData()
    {
        int randomIndex = GeneralUtilityMethods.GenerateRandomIndex(this.generatedAbilities.Count);
        return this.generatedAbilities[randomIndex];
    }

    private void DisplayAbilities(List<AbilityLibraryData.AbilityStats> chosenAbilities)
    {
        int cardIndex = CardUtilityMethods.GetCardIndex();

        Logger.Log($"Starting while loop while chosenAbilities.Count {chosenAbilities.Count} > cardIndex {cardIndex}", this);
        while (chosenAbilities.Count > cardIndex)
        {
            Logger.Log($"LOOPING.  chosenAbilities.Count {chosenAbilities.Count}, cardIndex - {cardIndex}", this);
            Logger.Log($"Card Panel to Edit - {this.shuffledList[cardIndex].name}", this);
            Logger.Log($"Ability to Assign to Card - {chosenAbilities[cardIndex].abilityName}");
            TextMeshProUGUI[] textElements = this.shuffledList[cardIndex].GetComponentsInChildren<TextMeshProUGUI>();
            textElements[0].text = "New Ability!!";
            textElements[1].text = chosenAbilities[cardIndex].abilityName.ToString();
            textElements[2].text = chosenAbilities[cardIndex].abilityDescription;

            abilityCardRelationship.Add(this.shuffledList[cardIndex], chosenAbilities[cardIndex]);

            cardIndex++;
            CardUtilityMethods.SetCardIndex(cardIndex);
        }

        Logger.Log($"While loop ended.  Final CardIndex Value - {CardUtilityMethods.GetCardIndex()}", this);
    }
}
