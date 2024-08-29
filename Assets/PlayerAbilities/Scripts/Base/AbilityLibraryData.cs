using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Game/AbilityData", order = 2)]
public class AbilityLibraryData : ScriptableObject
{
    [System.Serializable]
    public class AbilityStats
    {
        public string abilityName;
        public string abilityDescription;
        public int damage;
        public int fireRate;
        public AbilityUpgrades[] abilityUpgrades;
        public GameObject prefab;
        public PlayerAbilities playerAbilities;
    }

    public AbilityStats[] abilityStatsArray;
}
