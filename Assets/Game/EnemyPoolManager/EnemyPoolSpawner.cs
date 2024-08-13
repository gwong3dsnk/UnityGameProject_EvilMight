using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemyPoolSpawner : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float timeBetweenEnemies = 1f;
    [SerializeField] [Range(0f, 5f)] float timeBetweenWaves = 3f;    
    [SerializeField] Camera mainCamera;
    [SerializeField] GridManager gridManager;
    private Vector3 spawnLocation;
    private EnemyPool enemyPool;

    private void Start()
    {
        enemyPool = GetComponent<EnemyPool>();

        if (enemyPool == null)
        {
            Debug.LogError("Missing EnemyPool script component.", this);
        }

        if (gridManager == null)
        {
            Debug.LogError("Missing GridManager script assignment", this);
        }
        
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        for (int currentWave = 0; currentWave < enemyPool.WaveDataSets.Count; currentWave++)
        {
            yield return StartCoroutine(SpawnEnemies(currentWave));

            if (currentWave < enemyPool.WaveDataSets.Count - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    private IEnumerator SpawnEnemies(int currentWave)
    {
        string waveKey = $"Wave{currentWave + 1}";
        if (!enemyPool.WaveEnemies.ContainsKey(waveKey)) yield break;

        List<GameObject> enemiesToActivate = enemyPool.WaveEnemies[waveKey];
        Debug.Log($"WaveEnemyCount: {enemiesToActivate.Count}");

        foreach (GameObject enemy in enemiesToActivate)
        {
            if (!enemy.activeInHierarchy)
            {
                GenerateRandomSpawnLocation();
                enemy.transform.position = spawnLocation;

                // Add enemy to grid manager before activating it.
                gridManager.AddEnemy(enemy.GetComponent<Collider>());

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
