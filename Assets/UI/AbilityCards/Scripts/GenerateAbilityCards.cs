using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAbilityCards : MonoBehaviour
{
    List<AbilityLibraryData.AbilityStats> abilityData = new List<AbilityLibraryData.AbilityStats>();

    public List<AbilityLibraryData.AbilityStats> StartGeneratingAbilityCards(AbilityLibraryData abilityLibraryData)
    {
        CreateListOfAvailableAbilities(abilityLibraryData);
        List<AbilityLibraryData.AbilityStats> newAbilities = CreateNewAbilityList();

        return newAbilities;
    }

    /// <summary>
    /// Parses through the <see cref="AbilityLibraryData.AbilityStats"/> array in the given ability library data
    /// and creates a list of abilities that have not yet been unlocked by the player.
    /// </summary>
    /// <param name="abilityLibraryData"></param>
    private void CreateListOfAvailableAbilities(AbilityLibraryData abilityLibraryData)
    {
        abilityData.Clear();
        List<PlayerAbilities> activePlayerAbilities = PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities;

        for (int i = 0; i < abilityLibraryData.abilityStatsArray.Length; i++)
        {
            bool isFound = false;

            foreach (PlayerAbilities ability in activePlayerAbilities)
            {
                if (ability.name.Contains(abilityLibraryData.abilityStatsArray[i].playerAbilities.name))
                {
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                if (!abilityData.Contains(abilityLibraryData.abilityStatsArray[i]))
                {
                    abilityData.Add(abilityLibraryData.abilityStatsArray[i]);
                }
            }
        }
    }

    /// <summary>
    /// Parses through abilityData from <see cref="CreateListOfAvailableAbilities"/> and picks out 3 random
    /// abilities.  We never display more than 3 cards on player levelup, so we only need to pick 3 here.
    /// </summary>
    /// <returns>List of 3 randomly chosen abilities</returns>
    private List<AbilityLibraryData.AbilityStats> CreateNewAbilityList()
    {
        List<AbilityLibraryData.AbilityStats> newAbilityList = new List<AbilityLibraryData.AbilityStats>();

        if (abilityData.Count <= 3)
        {
            return abilityData;
        }
        else
        {
            int x = 0;

            while (x < 3)
            {
                int index = GeneralUtilityMethods.GenerateRandomIndex(abilityData.Count);

                if (!newAbilityList.Contains(abilityData[index]))
                {
                    newAbilityList.Add(abilityData[index]);
                }
                else
                {
                    while (newAbilityList.Contains(abilityData[index]))
                    {
                        index = GeneralUtilityMethods.GenerateRandomIndex(abilityData.Count);
                    }

                    newAbilityList.Add(abilityData[index]);
                }

                x++;
            }

            return newAbilityList;
        }
    }
}

