using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles retrieving every enemy prefab based on class/difficulty and stores all the data into a dictionary so they are cached.
/// </summary>
public class EnemyPrefabCache
{
    private Dictionary<(EnemyClass, EnemyDifficulty), GameObject> prefabDictionary;

    public EnemyPrefabCache(EnemyData.EnemyStats[] enemyStats)
    {
        prefabDictionary = new Dictionary<(EnemyClass, EnemyDifficulty), GameObject>();

        foreach (var stats in enemyStats)
        {
            var key = (stats.enemyClass, stats.difficulty);
            prefabDictionary[key] = stats.prefab;
        }
    }

    public GameObject GetEnemyPrefab(EnemyClass enemyClass, EnemyDifficulty enemyDifficulty)
    {
        prefabDictionary.TryGetValue((enemyClass, enemyDifficulty), out var prefab);
        return prefab;
    }
}
