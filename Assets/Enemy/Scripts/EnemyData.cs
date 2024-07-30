using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [System.Serializable]
    public class EnemyStats
    {
        public EnemyClass enemyClass;
        public EnemyDifficulty difficulty;
        public int hp;
        public int def;
        public int atk;
        public int spellAtk;
        public float atkRadius;
        public float movementSpeed;
    }

    public EnemyStats[] enemyStatsArray;    
}
