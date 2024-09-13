using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

/// <summary>
/// EDITOR ONLY
/// Sets up the 3-button GUI in UpgradeData asset under Ability Name Management section
/// Each button corresponds to one of the three defined methods.
/// </summary>
[CustomEditor(typeof(UpgradeLibraryData))]
public class UpgradesLibraryDataEditor : BaseLibraryDataEditor<UpgradeLibraryData>
{
    private UpgradeLibraryData upgradeData; 
    private string upgradeDataPath = "Assets/PlayerAbilities/Scripts/Data/UpgradeData.asset";

    private void OnEnable() 
    {
        // upgradeDataPath = "Assets/PlayerAbilities/Scripts/Data/UpgradeData.asset";
        upgradeData = AssetDatabase.LoadAssetAtPath<UpgradeLibraryData>(upgradeDataPath);
    }

    /// <summary>
    /// Called when button with label "1. Save Ability Names" is clicked.  
    /// Saves current ability name data to file.
    /// </summary>
    protected override void SaveNamesToFile()
    {
        StringBuilder sb = new StringBuilder();
        File.WriteAllText(abilityNameFilePath, ""); // empty the file

        Logger.Log("Starting to save current ability choices in upgradeData to abilityNames.txt file...");

        if (upgradeData != null)
        {
            foreach (var stats in upgradeData.upgradeStatsData)
            {
                sb.AppendLine(stats.parentAbility.ToString());
            }

            File.WriteAllText(abilityNameFilePath, sb.ToString());
        }
        else
        {
            Logger.Log("Unable to locate upgradeData asset");
        }

        Logger.Log("Finished saving current ability choices in upgradeData to abilityNames.txt file");
    }

    /// <summary>
    /// Called when button with text "2. Regenerate Ability Names" is clicked.
    /// New ability prefabs are discovered and written to the ability names text file.
    /// </summary>
    protected override void RegenerateAbilityNames()
    {
        base.RegenerateAbilityNames();
    }    

    /// <summary>
    /// Called when button with text "3. Load Ability Names" is clicked.
    /// Sets the data's ability name options to the new list
    /// </summary>    
    protected override void LoadAbilityNames()
    {
        var abilityNamesEnum = Enum.GetValues(typeof(AbilityNames)).Cast<AbilityNames>().ToArray();
        string[] previousNames = File.ReadAllLines(abilityNameFilePath);

        Logger.Log("Starting to load ability choices from abilityNames.txt to upgradeData parent ability...");

        if (upgradeData.upgradeStatsData.Length == previousNames.Length)
        {
            var abilityNameDictionary = abilityNamesEnum.ToDictionary(aName => aName.ToString(), aName => aName);
            
            for (int i = 0; i < previousNames.Length; i++)
            {
                if (abilityNameDictionary.TryGetValue(previousNames[i], out var matchedAbility))
                {
                    upgradeData.upgradeStatsData[i].parentAbility = matchedAbility;
                }
                else
                {
                    upgradeData.upgradeStatsData[i].parentAbility = abilityNamesEnum[0];
                }
            }
        }

        Logger.Log("Finished loading ability choices from abilityNames.txt to upgradeData parent ability");
    }
}
