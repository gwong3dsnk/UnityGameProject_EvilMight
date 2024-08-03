using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // [SerializeField] GameObject enemyMinion;
    [SerializeField] [Range(0f, 5f)] float timeBetweenEnemies = 1f;
    [SerializeField] [Range(0f, 5f)] float timeBetweenWaves = 3f;
    [SerializeField] Camera mainCamera;

    private Vector3 spawnLocation;
    private Dictionary<string, EnemyWaveData[]> waveDataSets = new Dictionary<string, EnemyWaveData[]>();
    private Dictionary<string, List<GameObject>> waveEnemies = new Dictionary<string, List<GameObject>>();
    private GameObject enemyToInstantiate;
    private List<GameObject> enemies = new List<GameObject>();
    private GameObject waveChildContainer;

    private void Awake()
    {
        CreateWaveDataRelationship();
        PopulateEnemyPool();
    }

    private void CreateWaveDataRelationship()
    {
        EnemyWaveController enemyWaveController = GetComponent<EnemyWaveController>();

        if (enemyWaveController!= null)
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
            Debug.LogError("Reference to Enemy Wave Controller script not found.", this);
        }
    }

    private void PopulateEnemyPool()
    {
        EnemyPrefabLibrary enemyPrefabLibrary = GetComponent<EnemyPrefabLibrary>();

        if (enemyPrefabLibrary != null)
        {
            EnemyPrefabAssignments[] enemyPrefabAssignments = enemyPrefabLibrary.EnemyPrefabAssignments;
            
            foreach (KeyValuePair<string, EnemyWaveData[]> wave in waveDataSets)
            {
                waveChildContainer = new GameObject(wave.Key);
                waveChildContainer.transform.parent = transform;

                foreach (EnemyWaveData data in wave.Value)
                {
                    // Start by finding the right enemy prefab that's to be instantiated, then instantiate it.
                    GetPrefabToInstantiate(enemyPrefabAssignments, data);

                    if (enemyToInstantiate == null)
                    {
                        Debug.LogError($"No prefab found for enemy class {data.enemyClass} and difficulty {data.enemyDifficulty}.", this);
                        continue;
                    }
                                        
                    InstantiateEnemyPrefab(wave, data);
                }

                // Add to list to later use to enable enemies during gameplay.
                waveEnemies.Add(wave.Key, enemies);
            }
        }
        else
        {
            Debug.LogError("Reference to EnemyPrefabLibrary script component is not found.", this);
        }
    }

    private void GetPrefabToInstantiate(EnemyPrefabAssignments[] enemyPrefabAssignments, EnemyWaveData data)
    {
        for (int i = 0; i < enemyPrefabAssignments.Length; i++)
        {
            if (data.enemyClass == enemyPrefabAssignments[i].enemyClass && data.enemyDifficulty == enemyPrefabAssignments[i].enemyDifficulty)
            {
                enemyToInstantiate = enemyPrefabAssignments[i].enemyPrefab;
                break;
            }
        }
    }

    private void InstantiateEnemyPrefab(KeyValuePair<string, EnemyWaveData[]> wave, EnemyWaveData data)
    {
        for (int i = 0; i < data.enemyCount; i++)
        {
            GameObject enemyUnit = Instantiate(enemyToInstantiate, transform.position, Quaternion.identity, waveChildContainer.transform);
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
                break;
            case 1: // spawn along the bottom edge
                spawnLocation = new Vector3(
                    UnityEngine.Random.Range(cameraBoundsMin.x - verticalSpawnOffset, cameraBoundsMax.x + verticalSpawnOffset), 
                    0f, 
                    -cameraBoundsMin.y
                );
                break;
            case 2: // spawn along the right edge 
                spawnLocation = new Vector3(
                    cameraBoundsMax.x + horizontalSpawnOffset,
                    0f, 
                    UnityEngine.Random.Range(cameraBoundsMin.y - horizontalSpawnOffset, cameraBoundsMax.y)
                );
                break;
            case 3: // spawn along the left edge
                spawnLocation = new Vector3(
                    cameraBoundsMax.x - horizontalSpawnOffset,
                    0f, 
                    UnityEngine.Random.Range(cameraBoundsMin.y - horizontalSpawnOffset, cameraBoundsMax.y)
                );
                break;                                
        }
    }    
}
