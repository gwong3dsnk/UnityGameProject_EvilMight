using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Game/AbilityData", order = 1)]
public class AbilityLibraryData : ScriptableObject
{
    [System.Serializable]
    public class AbilityStats
    {
        public AbilityNames abilityName;
        public string abilityDescription;
        public int damage;
        public int fireRate;
        // public AbilityUpgrades[] abilityUpgrades;
        public GameObject prefab;
        public PlayerAbilities playerAbilities;
    }

    private string abilityPrefabPath = "Assets/PlayerAbilities/Prefabs";
    private string enumFilePath = "Assets/PlayerAbilities/Scripts/Data/AbilityNames.cs";
    public AbilityStats[] abilityStatsArray;

    /// <summary>
    /// Called when the user clicks on the button in the UpgradeLibraryData ScriptableObject.
    /// </summary>
    public void RegenerateAbilityNames()
    {
        List<string> abilityNames = DataUtilityMethods.GetAssetPrefabNamesByPath(abilityPrefabPath);
        DataUtilityMethods.WriteNewAbilityEnums(abilityNames, enumFilePath);
    }    
}
