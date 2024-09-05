using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/UpgradeData", fileName = "UpgradeData", order = 2)]
public class UpgradeLibraryData : ScriptableObject
{
    [System.Serializable]
    public class UpgradeStats
    {
        public AbilityNames parentAbility;
        public UpgradeTypeData[] upgradeType;
    }

    private string abilityPrefabPath = "Assets/PlayerAbilities/Prefabs";
    private string enumFilePath = "Assets/PlayerAbilities/Scripts/Data/AbilityNames.cs";
    public UpgradeStats[] upgradeStatsData;

    /// <summary>
    /// Called when the user clicks on the button in the UpgradeLibraryData ScriptableObject.
    /// </summary>
    public void RegenerateAbilityNames()
    {
        List<string> abilityNames = DataUtilityMethods.GetAssetPrefabNamesByPath(abilityPrefabPath);
        DataUtilityMethods.WriteNewAbilityEnums(abilityNames, enumFilePath);
    }
}
