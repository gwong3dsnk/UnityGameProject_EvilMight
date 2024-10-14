using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

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
    private Dictionary<string, EnemyWaveData[]> waveDataSets = new();
    private Dictionary<string, List<GameObject>> waveEnemies = new();
    private GameObject waveContainer;
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
            waveContainer = new GameObject(wave.Key) { transform = { parent = transform } };

            foreach (EnemyWaveData data in wave.Value)
            {
                // Start by finding the right enemy prefab that's to be instantiated, then instantiate it.
                GameObject enemyToInstantiate = GetPrefabToInstantiate(data);

                if (enemyToInstantiate == null)
                {
                    Logger.LogError($"{this.name} - No prefab found for enemy class {data.enemyClass} and difficulty {data.enemyDifficulty}.", this);
                    continue;
                }
                                    
                enemies = InstantiateEnemyPrefab(data, enemyToInstantiate);
            }

            // Add to list to later use to enable enemies during gameplay.
            waveEnemies.Add(wave.Key, enemies);
        }
    }

    private GameObject GetPrefabToInstantiate(EnemyWaveData data)
    {
        EnemyData.EnemyStats stats = enemyStats.FirstOrDefault
        (
            prefabObject => data.enemyClass == prefabObject.enemyClass 
            && data.enemyDifficulty == prefabObject.difficulty
        );

        return stats?.prefab;
    }

    private List<GameObject> InstantiateEnemyPrefab(EnemyWaveData data, GameObject enemyToInstantiate)
    {   
        List<GameObject> enemies = new List<GameObject>();

        for (int i = 0; i < data.enemyCount; i++)
        {
            GameObject enemyUnit = Instantiate(enemyToInstantiate, transform.position, Quaternion.identity, waveContainer.transform);
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
