using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int playerHealth = 3;
    private Enemy enemy;

    private void OnParticleCollision(GameObject other) 
    {
        GameObject enemyPrefab = other.transform.parent?.gameObject;
        enemy = enemyPrefab.GetComponent<Enemy>();

        ProcessHit();
    }

    private void ProcessHit()
    {
        playerHealth -= enemy.Attack;
        if (playerHealth <= 0)
        {
            HandlePlayerDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        GetComponent<PlayerDeathHandler>().HandleDeath();
    }
}
