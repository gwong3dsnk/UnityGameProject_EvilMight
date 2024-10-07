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
        public GameObject prefab;
        public AbilityBase playerAbilities;
    }

    public AbilityStats[] abilityStatsArray;
}
