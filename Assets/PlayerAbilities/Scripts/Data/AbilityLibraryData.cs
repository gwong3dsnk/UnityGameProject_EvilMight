using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Game/AbilityData", order = 1)]
public class AbilityLibraryData : ScriptableObject
{
    [System.Serializable]
    public class AbilityStats
    {
        public AbilityNames abilityName;
        public string abilityDescription;
        public float damage;
        public float abilityCooldown;
        public float attackSpeed;
        public float abilityRange;
    }

    public AbilityStats[] abilityStatsArray;
}
