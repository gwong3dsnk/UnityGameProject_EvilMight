using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocator : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float searchTimeInterval = 0.5f;
    [SerializeField] private float searchRadius = 30f;
    private Collider nearestEnemy;
    public Collider NearestEnemy => nearestEnemy;
    // public static EnemyLocator EnemyLocatorInstance { get; private set; }

    // private void Awake()
    // {
    //     if (EnemyLocatorInstance == null)
    //     {
    //         EnemyLocatorInstance = this;
    //     }
    // }

    private void Start()
    {
        if (gridManager == null)
        {
            Debug.LogError("Missing Grid Manager reference", this);
        }

        StartCoroutine(FindNearestEnemy());
    }

    private IEnumerator FindNearestEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(searchTimeInterval);

            nearestEnemy = gridManager.GetNearestEnemy(transform.position, searchRadius);
        }
    }
}
