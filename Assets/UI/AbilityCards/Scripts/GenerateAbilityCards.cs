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

    private void CreateListOfAvailableAbilities(AbilityLibraryData abilityLibraryData)
    {
        abilityData.Clear();

        foreach (AbilityLibraryData.AbilityStats data in abilityLibraryData.abilityStatsArray)
        {
            foreach (PlayerAbilities activeAbility in PlayerAbilitiesManager.AbilityManagerInstance.ActiveAbilities)
            {
                if (!activeAbility.name.Contains(data.playerAbilities.name))
                {
                    abilityData.Add(data);
                }
            }
        }
    }

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
                int index = BaseUtilityMethods.GenerateRandomIndex(abilityData.Count);

                if (!newAbilityList.Contains(abilityData[index]))
                {
                    newAbilityList.Add(abilityData[index]);
                }
                else
                {
                    while (newAbilityList.Contains(abilityData[index]))
                    {
                        index = BaseUtilityMethods.GenerateRandomIndex(abilityData.Count);
                    }

                    newAbilityList.Add(abilityData[index]);
                }

                x++;
            }

            return newAbilityList;
        }
    }    
}

