using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWaveController))]
public class EnemyPool : MonoBehaviour
{
    #region Fields and Properties
    // SerializedFields
    [SerializeField] private EnemyData enemyData;

    // Public Fields/Properties/Events
    public Dictionary<string, EnemyWaveData[]> WaveDataSets => waveDataSets;
    public Dictionary<string, List<GameObject>> WaveEnemies => waveEnemies;

    // Private Fields/Properties
    private Dictionary<string, EnemyWaveData[]> waveDataSets = new Dictionary<string, EnemyWaveData[]>();
    private Dictionary<string, List<GameObject>> waveEnemies = new Dictionary<string, List<GameObject>>();
    private GameObject enemyToInstantiate;
    private GameObject waveChildContainer;
    private EnemyData.EnemyStats[] enemyStats;
    #endregion

    private void Awake()
    {
        if (enemyData == null)
        {
            Logger.LogError($"{this.name} - Missing reference to EnemyData.", this);
            return;
        }

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
            Logger.LogError($"{this.name} - Reference to Enemy Wave Controller script not found.", this);
        }
    }

    private void PopulateEnemyPool()
    {
        enemyStats = enemyData.enemyStatsArray;
        
        foreach (KeyValuePair<string, EnemyWaveData[]> wave in waveDataSets)
        {
            List<GameObject> enemies = new List<GameObject>();
            waveChildContainer = new GameObject(wave.Key);
            waveChildContainer.transform.parent = transform;

            foreach (EnemyWaveData data in wave.Value)
            {
                // Start by finding the right enemy prefab that's to be instantiated, then instantiate it.
                GetPrefabToInstantiate(data);

                if (enemyToInstantiate == null)
                {
                    Logger.LogError($"{this.name} - No prefab found for enemy class {data.enemyClass} and difficulty {data.enemyDifficulty}.", this);
                    continue;
                }
                                    
                enemies = InstantiateEnemyPrefab(data);
            }

            // Add to list to later use to enable enemies during gameplay.
            waveEnemies.Add(wave.Key, enemies);
        }
    }

    private void GetPrefabToInstantiate(EnemyWaveData data)
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

    private List<GameObject> InstantiateEnemyPrefab(EnemyWaveData data)
    {   
        List<GameObject> enemies = new List<GameObject>();

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

        return enemies;
    }
}
