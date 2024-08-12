using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private GameObject bulletToInstantiate;
    private List<GameObject> bullets = new List<GameObject>();

    // private void Awake()
    // {
    //     PopulateBulletPool();
    // }

    // private void PopulateBulletPool()
    // {
    //     EnemyPrefabLibrary enemyPrefabLibrary = GetComponent<EnemyPrefabLibrary>();

    //     if (enemyPrefabLibrary != null)
    //     {
    //         EnemyPrefabAssignments[] enemyPrefabAssignments = enemyPrefabLibrary.EnemyPrefabAssignments;
            
    //         foreach (KeyValuePair<string, EnemyWaveData[]> wave in waveDataSets)
    //         {
    //             waveChildContainer = new GameObject(wave.Key);
    //             waveChildContainer.transform.parent = transform;

    //             foreach (EnemyWaveData data in wave.Value)
    //             {
    //                 // Start by finding the right enemy prefab that's to be instantiated, then instantiate it.
    //                 GetPrefabToInstantiate(enemyPrefabAssignments, data);

    //                 if (enemyToInstantiate == null)
    //                 {
    //                     Debug.LogError($"No prefab found for enemy class {data.enemyClass} and difficulty {data.enemyDifficulty}.", this);
    //                     continue;
    //                 }
                                        
    //                 InstantiateEnemyPrefab(wave, data);
    //             }

    //             // Add to list to later use to enable enemies during gameplay.
    //             waveEnemies.Add(wave.Key, enemies);
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("Reference to EnemyPrefabLibrary script component is not found.", this);
    //     }
    // }

    // private void GetPrefabToInstantiate(EnemyPrefabAssignments[] enemyPrefabAssignments, EnemyWaveData data)
    // {
    //     for (int i = 0; i < enemyPrefabAssignments.Length; i++)
    //     {
    //         if (data.enemyClass == enemyPrefabAssignments[i].enemyClass && data.enemyDifficulty == enemyPrefabAssignments[i].enemyDifficulty)
    //         {
    //             enemyToInstantiate = enemyPrefabAssignments[i].enemyPrefab;
    //             break;
    //         }
    //     }
    // }

    // private void InstantiateEnemyPrefab(KeyValuePair<string, EnemyWaveData[]> wave, EnemyWaveData data)
    // {
    //     for (int i = 0; i < data.enemyCount; i++)
    //     {
    //         GameObject enemyUnit = Instantiate(enemyToInstantiate, transform.position, Quaternion.identity, waveChildContainer.transform);
    //         Enemy enemyScript = enemyUnit.GetComponent<Enemy>();

    //         // Based on enemy class/difficulty pair, set the enemy properties.
    //         if (enemyScript != null)
    //         {
    //             enemyScript.SetClassAndDifficulty(data.enemyClass, data.enemyDifficulty);
    //         }

    //         enemyUnit.SetActive(false);

    //         enemies.Add(enemyUnit);
    //     }        
    // }
}
