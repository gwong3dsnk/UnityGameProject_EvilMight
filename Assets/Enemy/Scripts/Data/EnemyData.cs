using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    [System.Serializable]
    public class EnemyStats
    {
        public EnemyClass enemyClass;
        public EnemyDifficulty difficulty;
        public int hp;
        public int atk;
        public float atkRadius;
        public float movementSpeed;
        public int experience;
        public GameObject prefab;
    }

    public EnemyStats[] enemyStatsArray;    
}
