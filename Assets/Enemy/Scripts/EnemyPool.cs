using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyMinion;
    [SerializeField] [Range(0f, 5f)] float timeBetweenEnemies = 1f;
    [SerializeField] [Range(0f, 5f)] float timeBetweenWaves = 3f;
    [SerializeField] Camera mainCamera;

    private Vector3 spawnLocation;
    private Dictionary<string, EnemyWaveData[]> waveDataSets = new Dictionary<string, EnemyWaveData[]>();
    private Dictionary<string, List<GameObject>> waveEnemies = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        CreateWaveDataRelationship();
        PopulateEnemyPool();
    }

    private void CreateWaveDataRelationship()
    {
        EnemyWaveController enemyWaveController = GetComponent<EnemyWaveController>();
        EnemyWaveContainer[] waveContainers = enemyWaveController.EnemyWaveContainer;
        int waveNumber = 1;

        foreach (EnemyWaveContainer container in waveContainers)
        {
            waveDataSets.Add($"Wave{waveNumber}", container.enemyWaveData);
            waveNumber++;
        }
    }

    private void PopulateEnemyPool()
    {
        foreach (KeyValuePair<string, EnemyWaveData[]> wave in waveDataSets)
        {
            // Create individual wave gameobjects parented to EnemyPool
            GameObject waveChildContainer = new GameObject(wave.Key);
            List<GameObject> enemies = new List<GameObject>();
            waveChildContainer.transform.parent = transform;

            // Loop through each element in Enemy Wave Data
            foreach (EnemyWaveData data in wave.Value)
            {
                // For each enemy data element, get and use enemy count to instantiate the number of enemy class/difficulty pair.
                for (int i = 0; i < data.enemyCount; i++)
                {
                    GameObject enemyUnit = Instantiate(enemyMinion, transform.position, Quaternion.identity, waveChildContainer.transform);
                    Enemy enemyScript = enemyUnit.GetComponent<Enemy>();

                    if (enemyScript != null)
                    {
                        enemyScript.SetClassAndDifficulty(data.enemyClass, data.enemyDifficulty);
                    }

                    enemyUnit.SetActive(false);

                    enemies.Add(enemyUnit);
                }
            }

            // Add to list to later use to enable enemies during gameplay.
            waveEnemies.Add(wave.Key, enemies);            
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        for (int currentWave = 0; currentWave < waveDataSets.Count; currentWave++)
        {
            yield return StartCoroutine(SpawnEnemies(currentWave));
            if (currentWave < waveDataSets.Count - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    private IEnumerator SpawnEnemies(int currentWave)
    {
        string waveKey = $"Wave{currentWave + 1}";
        if (!waveEnemies.ContainsKey(waveKey)) yield break;

        List<GameObject> enemiesToActivate = waveEnemies[waveKey];

        foreach (GameObject enemy in enemiesToActivate)
        {
            if (!enemy.activeInHierarchy)
            {
                GenerateRandomSpawnLocation();
                enemy.transform.position = spawnLocation;
                enemy.SetActive(true);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
    }

    private void GenerateRandomSpawnLocation()
    {
        float verticalSpawnOffset = 30f;
        float horizontalSpawnOffset = 60f;
        int spawningEdge = UnityEngine.Random.Range(0, 4);

        // Min returns (-0.31, 34.67, -10.09).  Max returns (0.31, 34.76, -9.76)
        Vector3 cameraBoundsMin = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane));
        Vector3 cameraBoundsMax = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));

        switch(spawningEdge)
        {
            case 0: // spawn along the top edge
                spawnLocation = new Vector3(
                    UnityEngine.Random.Range(cameraBoundsMin.x - verticalSpawnOffset, cameraBoundsMax.x + verticalSpawnOffset), 
                    0f, 
                    cameraBoundsMin.y
                );
                Debug.Log($"Spawning top edge at {spawnLocation}");
                break;
            case 1: // spawn along the bottom edge
                spawnLocation = new Vector3(
                    UnityEngine.Random.Range(cameraBoundsMin.x - verticalSpawnOffset, cameraBoundsMax.x + verticalSpawnOffset), 
                    0f, 
                    -cameraBoundsMin.y
                );
                Debug.Log($"Spawning bottom edge at {spawnLocation}");
                break;
            case 2: // spawn along the right edge 
                spawnLocation = new Vector3(
                    cameraBoundsMax.x + horizontalSpawnOffset,
                    0f, 
                    UnityEngine.Random.Range(cameraBoundsMin.y - horizontalSpawnOffset, cameraBoundsMax.y)
                );
                Debug.Log($"Spawning right edge at {spawnLocation}");
                break;
            case 3: // spawn along the left edge
                spawnLocation = new Vector3(
                    cameraBoundsMax.x - horizontalSpawnOffset,
                    0f, 
                    UnityEngine.Random.Range(cameraBoundsMin.y - horizontalSpawnOffset, cameraBoundsMax.y)
                );
                Debug.Log($"Spawning left edge at {spawnLocation}");
                break;                                
        }
    }    
}
