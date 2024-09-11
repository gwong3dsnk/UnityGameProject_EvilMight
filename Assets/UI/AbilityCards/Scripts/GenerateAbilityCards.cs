using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAbilityCards : MonoBehaviour
{
    [SerializeField] AbilityDatabaseManager abilityDatabaseManager;
    private List<AbilityLibraryData.AbilityStats> abilityDatabase;

    public List<AbilityLibraryData.AbilityStats> StartGeneratingAbilityCards()
    {
        abilityDatabase = abilityDatabaseManager.AbilityDatabase;
        List<AbilityLibraryData.AbilityStats> newAbilities = CreateNewAbilityList();

        return newAbilities;
    }

    /// <summary>
    /// Parses through abilityData from <see cref="CreateListOfAvailableAbilities"/> and picks out 3 random
    /// abilities.  We never display more than 3 cards on player levelup, so we only need to pick 3 here.
    /// </summary>
    /// <returns>List of 3 randomly chosen abilities</returns>
    private List<AbilityLibraryData.AbilityStats> CreateNewAbilityList()
    {
        List<AbilityLibraryData.AbilityStats> newAbilityList = new List<AbilityLibraryData.AbilityStats>();

        if (abilityDatabase.Count <= 3)
        {
            Logger.Log($"Returning abilityDatabase.  Count is - {abilityDatabase.Count}");
            return abilityDatabase;
        }
        else
        {
            int x = 0;

            while (x < 3)
            {
                int index = GeneralUtilityMethods.GenerateRandomIndex(abilityDatabase.Count);

                if (!newAbilityList.Contains(abilityDatabase[index]))
                {
                    newAbilityList.Add(abilityDatabase[index]);
                }
                else
                {
                    while (newAbilityList.Contains(abilityDatabase[index]))
                    {
                        index = GeneralUtilityMethods.GenerateRandomIndex(abilityDatabase.Count);
                    }

                    newAbilityList.Add(abilityDatabase[index]);
                }

                x++;
            }

            return newAbilityList;
        }
    }
}

