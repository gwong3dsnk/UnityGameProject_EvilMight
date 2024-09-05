using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(AbilityLibraryData))]
public class AbilityLibraryDataEditor : BaseLibraryDataEditor<AbilityLibraryData>
{
    private AbilityLibraryData abilityData;
    protected string abilityDataPath;

    private void OnEnable() 
    {
        abilityDataPath = "Assets/PlayerAbilities/Scripts/Data/AbilityData.asset";
        abilityData = AssetDatabase.LoadAssetAtPath<AbilityLibraryData>(abilityDataPath);
    }

    protected override void RegenerateAbilityNames(AbilityLibraryData libraryData)
    {
        libraryData.RegenerateAbilityNames();
        AssetDatabase.Refresh();
    }

    protected override void SaveNamesToFile()
    {
        StringBuilder sb = new StringBuilder();
        File.WriteAllText(abilityNameFilePath, ""); // empty the file

        Logger.Log("Starting to save current ability choices in upgradeData to abilityNames.txt file...");

        if (abilityData != null)
        {
            foreach (var stats in abilityData.abilityStatsArray)
            {
                sb.AppendLine(stats.abilityName.ToString());
            }

            File.WriteAllText(abilityNameFilePath, sb.ToString());
        }
        else
        {
            Logger.Log("Unable to locate upgradeData asset");
        }

        Logger.Log("Finished saving current ability choices in upgradeData to abilityNames.txt file");
    }

    protected override void LoadAbilityNames()
    {
        var abilityNamesEnum = Enum.GetValues(typeof(AbilityNames)).Cast<AbilityNames>().ToArray();
        string[] previousNames = File.ReadAllLines(abilityNameFilePath);

        Logger.Log("Starting to load ability choices from abilityNames.txt to upgradeData parent ability...");

        if (abilityData.abilityStatsArray.Length == previousNames.Length)
        {
            var abilityNameDictionary = abilityNamesEnum.ToDictionary(aName => aName.ToString(), aName => aName);
            
            for (int i = 0; i < previousNames.Length; i++)
            {
                if (abilityNameDictionary.TryGetValue(previousNames[i], out var matchedAbility))
                {
                    abilityData.abilityStatsArray[i].abilityName = matchedAbility;
                }
                else
                {
                    abilityData.abilityStatsArray[i].abilityName = abilityNamesEnum[0];
                }
            }
        }

        Logger.Log("Finished loading ability choices from abilityNames.txt to upgradeData parent ability");
    }
}
