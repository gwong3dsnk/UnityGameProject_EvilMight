using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private EnemyPrefabCache enemyPrefabCache;
    private Queue<GameObject> enemyQueue;
    private Transform instantiatePosition;

    public void Initialize(EnemyData.EnemyStats[] enemyStats)
    {
        enemyPrefabCache = new EnemyPrefabCache(enemyStats);
    }

    public Queue<GameObject> CreateEnemy(EnemyWaveData enemyWaveData, GameObject waveContainer)
    {
        enemyQueue = new Queue<GameObject>();
        instantiatePosition = waveContainer.transform;

        if (enemyWaveData.enemyClass == EnemyClass.Melee)
        {
            CreateMeleeEnemy(enemyWaveData);
        }
        else
        {
            CreateRangedEnemy(enemyWaveData);
        }

        return enemyQueue;
    }

    public void CreateRangedEnemy(EnemyWaveData enemyWaveData)
    {
        GameObject enemyPrefab;

        switch (enemyWaveData.enemyDifficulty)
        {
            case EnemyDifficulty.Easy:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Range, EnemyDifficulty.Easy);
                InstantiateEnemy(enemyWaveData, enemyPrefab);
                break;
            case EnemyDifficulty.Normal:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Range, EnemyDifficulty.Normal);
                InstantiateEnemy(enemyWaveData, enemyPrefab);
                break;
            case EnemyDifficulty.Hard:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Range, EnemyDifficulty.Hard);
                InstantiateEnemy(enemyWaveData, enemyPrefab);
                break;
            default:
                break;
        }
    }

    public void CreateMeleeEnemy(EnemyWaveData enemyWaveData)
    {
        GameObject enemyPrefab;

        switch (enemyWaveData.enemyDifficulty)
        {
            case EnemyDifficulty.Easy:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Melee, EnemyDifficulty.Easy);
                InstantiateEnemy(enemyWaveData, enemyPrefab);
                break;
            case EnemyDifficulty.Normal:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Melee, EnemyDifficulty.Normal);
                InstantiateEnemy(enemyWaveData, enemyPrefab);
                break;
            case EnemyDifficulty.Hard:
                enemyPrefab = enemyPrefabCache.GetEnemyPrefab(EnemyClass.Melee, EnemyDifficulty.Hard);
                InstantiateEnemy(enemyWaveData, enemyPrefab);
                break;
            default:
                break;
        }
    }    

    private void InstantiateEnemy(EnemyWaveData data, GameObject enemyPrefab)
    {   
        for (int i = 0; i < data.enemyCount; i++)
        {
            GameObject enemyUnit = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, instantiatePosition);

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
            enemyQueue.Enqueue(enemyUnit);
        }
    }    
}
