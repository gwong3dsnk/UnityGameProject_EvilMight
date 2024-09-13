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
        Logger.Log("Initializing ABILITY DATABASE MANAGER OnAwake", this);
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

        Logger.Log("Finished ability database initialization", this);
        Logger.Log("Logging Initialized Ability Database Contents:", this);
        foreach (var item in abilityDatabase)
        {
            Logger.Log($">>> {item.abilityName}");
        }
        Logger.Log("End Logging", this);
    }

    public void RemoveAbilityFromDatabase(PlayerAbilities unlockedAbility)
    {
        Logger.Log($"Starting RemoveAbilityFromDatabase to possibly remove {unlockedAbility.AbilityName} from ability database.", this);

        Logger.Log("Logging Ability Database Contents BEFORE removal in RemoveAbility method:", this);
        foreach (var item in abilityDatabase)
        {
            Logger.Log($">>> {item.abilityName}");
        }
        Logger.Log("End Logging", this);

        AbilityLibraryData.AbilityStats abilityToRemove = new AbilityLibraryData.AbilityStats();

        if (abilityDatabase.Count > 0)
        {
            foreach (var stat in abilityDatabase)
            {
                if (stat.abilityName == unlockedAbility.AbilityName)
                {
                    Logger.Log($"Ability to remove is {unlockedAbility.AbilityName}.  Verified.", this);
                    abilityToRemove = stat;
                    break;
                }
                else
                {
                    Logger.Log("Ability to remove is null, not removing anything", this);
                    abilityToRemove = null;
                }
            }

            if (abilityToRemove != null)
            {
                Logger.Log($"Starting removal of {abilityToRemove.abilityName}", this);
                abilityDatabase.Remove(abilityToRemove);

                Logger.Log("Logging Ability Database Contents AFTER removal:", this);
                foreach (var item in abilityDatabase)
                {
                    Logger.Log($">>> {item.abilityName}");
                }
                Logger.Log("End Logging", this);                
            }
        }
        else
        {
            Logger.Log("Ability database not yet populated.  Not processing any ability removals.", this);
        }
    }
}
