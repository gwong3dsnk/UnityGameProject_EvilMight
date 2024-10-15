using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemyPoolSpawner : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float timeBetweenEnemies = 1f;
    [SerializeField] [Range(0f, 5f)] float timeBetweenWaves = 3f;    
    [SerializeField] Camera mainCamera;
    private EnemyPool enemyPool;
    private Transform playerTransform;

    private void Start()
    {
        enemyPool = GetComponent<EnemyPool>();

        if (enemyPool == null || mainCamera == null)
        {
            Logger.LogError($"{this.name} - Missing EnemyPool component or reference to MainCamera.", this);
            return;
        }

        playerTransform = mainCamera.transform.parent.GetComponentInChildren<PlayerHealth>().transform;

        if (playerTransform == null)
        {
            Logger.LogError($"{this.name} - Unable to find PlayerHealth component on Player character.", this);
            return;
        }

        if (GridManager.GridManagerInstance == null)
        {
            Logger.LogError($"{this.name} - Unable to find GridManagerInstance", this);
            return;
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

        Queue<GameObject> enemiesToActivate = enemyPool.WaveEnemies[waveKey];

        while (enemiesToActivate.Count > 0)
        {
            GameObject enemy = enemyPool.WaveEnemies[waveKey].Dequeue();
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.position = BaseUtilityMethods.GenerateRandomSpawnLocation(playerTransform.position);

                // Add enemy to grid manager before activating it.
                GridManager.GridManagerInstance.AddEnemy(enemy.GetComponent<Collider>());

                enemy.SetActive(true);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
    }
}
