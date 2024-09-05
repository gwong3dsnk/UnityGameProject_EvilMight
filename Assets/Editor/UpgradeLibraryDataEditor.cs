using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(UpgradeLibraryData))]
public class UpgradesLibraryDataEditor : BaseLibraryDataEditor<UpgradeLibraryData>
{
    private UpgradeLibraryData upgradeData;
    private string upgradeDataPath;

    private void OnEnable() 
    {
        upgradeDataPath = "Assets/PlayerAbilities/Scripts/Data/UpgradeData.asset";
        upgradeData = AssetDatabase.LoadAssetAtPath<UpgradeLibraryData>(upgradeDataPath);
    }

    protected override void RegenerateAbilityNames(UpgradeLibraryData libraryData)
    {
        libraryData.RegenerateAbilityNames();
        AssetDatabase.Refresh();
    }

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
