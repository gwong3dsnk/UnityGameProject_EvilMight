using System;
using System.Collections;
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
    public static int cardIndex;

    public void ProcessAbilityDisplay(List<GameObject> shuffledList, int finalNumOfAbilitiesToDisplay, List<AbilityLibraryData.AbilityStats> generatedAbilities)
    {
        this.shuffledList = shuffledList;
        this.generatedAbilities = generatedAbilities;
        List<AbilityLibraryData.AbilityStats> chosenAbilities = SelectRandomAbilitiesToDisplay(finalNumOfAbilitiesToDisplay);
        
        if (chosenAbilities.Count > 0)
        {
            DisplayAbilities(chosenAbilities);
        }
        else
        {
            Debug.Log("All abilities have been unlocked.  No new abilities to display.", this);
        }
    }

    private List<AbilityLibraryData.AbilityStats> SelectRandomAbilitiesToDisplay(int numToSelect)
    {
        List<AbilityLibraryData.AbilityStats> randomlySelectedAbilities = new List<AbilityLibraryData.AbilityStats>();
        AbilityLibraryData.AbilityStats chosenAbility;

        if (this.generatedAbilities.Count > 0)
        {
            chosenAbility = GetRandomAbilityData();
        }
        else
        {
            chosenAbility = null;
        }

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
        int randomIndex = BaseUtilityMethods.GenerateRandomIndex(this.generatedAbilities.Count);
        return this.generatedAbilities[randomIndex];
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
}
