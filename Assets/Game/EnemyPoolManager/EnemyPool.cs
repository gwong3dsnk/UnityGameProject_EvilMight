using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWaveController))]
public class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    private Dictionary<string, EnemyWaveData[]> waveDataSets = new Dictionary<string, EnemyWaveData[]>();
    public Dictionary<string, EnemyWaveData[]> WaveDataSets => waveDataSets;
    private Dictionary<string, List<GameObject>> waveEnemies = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<GameObject>> WaveEnemies => waveEnemies;
    private GameObject enemyToInstantiate;
    private GameObject waveChildContainer;

    private void Awake()
    {
        CreateWaveDataRelationship();
        PopulateEnemyPool();
    }

    private void CreateWaveDataRelationship()
    {
        EnemyWaveController enemyWaveController = GetComponent<EnemyWaveController>();

        if (enemyWaveController != null)
        {
            EnemyWaveContainer[] waveContainers = enemyWaveController.EnemyWaveContainer;
            int waveNumber = 1;

            foreach (EnemyWaveContainer container in waveContainers)
            {
                waveDataSets.Add($"Wave{waveNumber}", container.enemyWaveData);
                waveNumber++;
            }
        }
        else
        {
            Logger.LogError("Reference to Enemy Wave Controller script not found.", this);
        }
    }

    private void PopulateEnemyPool()
    {
        if (enemyData != null)
        {
            EnemyData.EnemyStats[] enemyStats = enemyData.enemyStatsArray;

            // WIP - replacing enemy prefab assignments with enmy data's prefab property.
            
            foreach (KeyValuePair<string, EnemyWaveData[]> wave in waveDataSets)
            {
                List<GameObject> enemies = new List<GameObject>();
                waveChildContainer = new GameObject(wave.Key);
                waveChildContainer.transform.parent = transform;

                foreach (EnemyWaveData data in wave.Value)
                {
                    // Start by finding the right enemy prefab that's to be instantiated, then instantiate it.
                    GetPrefabToInstantiate(enemyStats, data);

                    if (enemyToInstantiate == null)
                    {
                        Logger.LogError($"No prefab found for enemy class {data.enemyClass} and difficulty {data.enemyDifficulty}.", this);
                        continue;
                    }
                                        
                    InstantiateEnemyPrefab(wave, data, enemies);
                }

                // Add to list to later use to enable enemies during gameplay.
                waveEnemies.Add(wave.Key, enemies);
            }
        }
        else
        {
            Logger.LogError("Reference to Enemy Data script component is not found.", this);
        }
    }

    private void GetPrefabToInstantiate(EnemyData.EnemyStats[] enemyStats, EnemyWaveData data)
    {
        for (int i = 0; i < enemyStats.Length; i++)
        {
            if (data.enemyClass == enemyStats[i].enemyClass && data.enemyDifficulty == enemyStats[i].difficulty)
            {
                enemyToInstantiate = enemyStats[i].prefab;
                break;
            }
        }
    }

    private void InstantiateEnemyPrefab(KeyValuePair<string, EnemyWaveData[]> wave, EnemyWaveData data, List<GameObject> enemies)
    {   
        for (int i = 0; i < data.enemyCount; i++)
        {
            GameObject enemyUnit = Instantiate(enemyToInstantiate, transform.position, Quaternion.identity, waveChildContainer.transform);
            enemyUnit.name = $"Enemy{i}";
            Enemy enemyScript = enemyUnit.GetComponent<Enemy>();

            // Based on enemy class/difficulty pair, set the enemy properties.
            if (enemyScript != null)
            {
                enemyScript.SetClassAndDifficulty(data.enemyClass, data.enemyDifficulty);
            }

            enemyUnit.SetActive(false);

            enemies.Add(enemyUnit);
        }        
    }
}
