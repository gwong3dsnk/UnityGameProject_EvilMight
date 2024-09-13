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

    public UpgradeStats[] upgradeStatsData;
}
