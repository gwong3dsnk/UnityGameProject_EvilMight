using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/UpgradeData", fileName = "UpgradeData", order = 2)]
public class AbilityUpgradesLibraryData : ScriptableObject
{
    [System.Serializable]
    public class UpgradeStats
    {
        public AbilityNames parentAbility;
        public UpgradeTypeData[] upgradeType;
    }

    public UpgradeStats[] upgradeStatsData;
}
