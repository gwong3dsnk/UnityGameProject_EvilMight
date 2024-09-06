using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNearestEnemy : MonoBehaviour
{
    private EnemyLocator enemyLocator;
    private float rotationSpeed = 10f;

    // void Start()
    // {
    //     if (EnemyLocator.EnemyLocatorInstance != null)
    //     {
    //         enemyLocator = EnemyLocator.EnemyLocatorInstance;
    //     }
    //     else
    //     {
    //         Logger.LogError("EnemyLocator Instance not found!", this);
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        if (enemyLocator != null)
        {
            Collider nearestEnemy = enemyLocator.NearestEnemy;
            if (nearestEnemy != null)
            {
                Vector3 nearestEnemyPosition = nearestEnemy.transform.position;
                // Logger.Log($"Enemy Position - {nearestEnemyPosition}");
                Vector3 direction = (nearestEnemyPosition - transform.position).normalized;
                // Logger.Log($"Enemy Direction - {direction}");

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);             
            }
        }
    }
}
