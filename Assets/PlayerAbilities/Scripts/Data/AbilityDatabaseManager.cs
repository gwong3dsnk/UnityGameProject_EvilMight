using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDatabaseManager : MonoBehaviour
{
    [SerializeField] AbilityLibraryData abilityLibraryData;
    private List<AbilityLibraryData.AbilityStats> abilityDatabase = new List<AbilityLibraryData.AbilityStats>();
    public List<AbilityLibraryData.AbilityStats> AbilityDatabase => abilityDatabase;

    private void Awake() 
    {
        InitializeAbilityDatabase();
    }

    /// <summary>
    /// Parses through the <see cref="AbilityLibraryData.AbilityStats"/> array in the given ability library data
    /// and creates a list of abilities that have not yet been unlocked by the player.
    /// </summary>
    /// <param name="abilityLibraryData"></param>
    private void InitializeAbilityDatabase()
    {
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
                if (!abilityDatabase.Contains(abilityLibraryData.abilityStatsArray[i]))
                {
                    abilityDatabase.Add(abilityLibraryData.abilityStatsArray[i]);
                }
            }
        }
    }

    public void RemoveAbilityFromDatabase(PlayerAbilities unlockedAbility)
    {
        AbilityLibraryData.AbilityStats abilityToRemove = new AbilityLibraryData.AbilityStats();;
        
        foreach (var stat in abilityDatabase)
        {
            if (stat.abilityName == unlockedAbility.AbilityName)
            {
                abilityToRemove = stat;
                break;
            }
            else
            {
                abilityToRemove = null;
            }
        }

        if (abilityToRemove != null)
        {
            abilityDatabase.Remove(abilityToRemove);
        }
    }
}
