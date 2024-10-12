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
            Logger.LogError($"{this.name} - Missing EnemyPool script component.", this);
        }

        if (GridManager.GridManagerInstance != null)
        {
            gridManager = GridManager.GridManagerInstance;
        }
        else
        {
            Logger.LogError($"{this.name} - Missing reference to GridManager singleton instance", this);
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

    /// <summary>
    /// Generate random world position where the enemy prefab will be placed before being set to active.
    /// </summary>
    private void GenerateRandomSpawnLocation()
    {
        Transform playerTransform = mainCamera.transform.parent.GetComponentInChildren<PlayerHealth>().transform;
        Vector2 random2DDirection = Random.insideUnitCircle.normalized;
        Vector3 direction3D = new Vector3(random2DDirection.x, 0, random2DDirection.y);
        float randomDistance = Random.Range(10.0f, 30.0f);
        spawnLocation = playerTransform.position + direction3D * randomDistance;
    }        
}
