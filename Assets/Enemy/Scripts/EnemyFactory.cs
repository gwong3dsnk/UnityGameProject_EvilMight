using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static Queue<GameObject> CreateRangedEnemy(EnemyWaveData enemyWaveData, EnemyData.EnemyStats[] enemyStats, GameObject waveContainer)
    {
        GameObject enemyPrefab;

        switch (enemyWaveData.enemyDifficulty)
        {
            case EnemyDifficulty.Easy:
                enemyPrefab = GetPrefabToInstantiate(enemyStats, EnemyClass.Range, EnemyDifficulty.Easy);
                return InstantiateEnemy(enemyWaveData, enemyPrefab, waveContainer);
            case EnemyDifficulty.Normal:
                enemyPrefab = GetPrefabToInstantiate(enemyStats, EnemyClass.Range, EnemyDifficulty.Normal);
                return InstantiateEnemy(enemyWaveData, enemyPrefab, waveContainer);
            case EnemyDifficulty.Hard:
                enemyPrefab = GetPrefabToInstantiate(enemyStats, EnemyClass.Range, EnemyDifficulty.Hard);
                return InstantiateEnemy(enemyWaveData, enemyPrefab, waveContainer);
            default:
                return null;
        }
    }

    private static GameObject GetPrefabToInstantiate(EnemyData.EnemyStats[] enemyStats, EnemyClass enemyClass, EnemyDifficulty enemyDifficulty)
    {
        EnemyData.EnemyStats stats = enemyStats.FirstOrDefault
        (
            stat => enemyClass == stat.enemyClass 
            && enemyDifficulty == stat.difficulty
        );

        return stats?.prefab;
    }    

    private static Queue<GameObject> InstantiateEnemy(EnemyWaveData data, GameObject enemyPrefab, GameObject waveContainer)
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
