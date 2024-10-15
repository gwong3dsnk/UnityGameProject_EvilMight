using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private EnemyPrefabCache enemyPrefabCache;

    public void Initialize(EnemyData.EnemyStats[] enemyStats)
    {
        enemyPrefabCache = new EnemyPrefabCache(enemyStats);
    }

    public Queue<GameObject> CreateRangedEnemy(EnemyWaveData enemyWaveData, EnemyData.EnemyStats[] enemyStats, GameObject waveContainer)
    {
        GameObject enemyPrefab;

        switch (enemyWaveData.enemyDifficulty)
        {
            case EnemyDifficulty.Easy:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Range, EnemyDifficulty.Easy);
                return InstantiateEnemy(enemyWaveData, enemyPrefab, waveContainer);
            case EnemyDifficulty.Normal:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Range, EnemyDifficulty.Normal);
                return InstantiateEnemy(enemyWaveData, enemyPrefab, waveContainer);
            case EnemyDifficulty.Hard:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Range, EnemyDifficulty.Hard);
                return InstantiateEnemy(enemyWaveData, enemyPrefab, waveContainer);
            default:
                return null;
        }
    }

    private Queue<GameObject> InstantiateEnemy(EnemyWaveData data, GameObject enemyPrefab, GameObject waveContainer)
    {   
        Queue<GameObject> enemies = new Queue<GameObject>();

        for (int i = 0; i < data.enemyCount; i++)
        {
            GameObject enemyUnit = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, waveContainer.transform);

            // Configure the instantiated enemy object
            enemyUnit.name = $"Enemy{i}";
            Enemy enemyScript = enemyUnit.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.SetClassAndDifficulty(data.enemyClass, data.enemyDifficulty);
            }
            else
            {
                Logger.LogError("Enemy gameobject is missing an Enemy script component.  Based on class/difficulty, there should be one.  I.e. RangedEnemy_Easy.");
            }

            enemyUnit.SetActive(false);
            enemies.Enqueue(enemyUnit);
        }        

        return enemies;
    }    
}
