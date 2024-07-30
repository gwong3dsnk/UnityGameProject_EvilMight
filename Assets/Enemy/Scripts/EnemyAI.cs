using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform player;
    NavMeshAgent navMeshAgent;
    Enemy enemy;
    float distanceToTarget = Mathf.Infinity;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        if (player == null)
        {
            Debug.LogError("PlayerMovement script is not found on any active gameObject in the scene.  Does the player gameobject exist?", this);
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError($"NavMeshAgent component is not found on {gameObject.name}", this);
        }

        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Enemy not found on enemy unit", this);
        }
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(player.position, transform.position);
        EngagePlayer();
    }

    private void EngagePlayer()
    {
        if (navMeshAgent == null) return;

        // Set the enemy traversal speed and stopping distance
        navMeshAgent.speed = enemy.MovementSpeed;
        navMeshAgent.stoppingDistance = enemy.AttackRadius;

        if (distanceToTarget >= navMeshAgent.stoppingDistance) // enemy will chase the player
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance) // enemy player within range, will attack
        {
            AttackTarget();
        }        
    }

    private void AttackTarget()
    {
        Debug.Log("Attacking the player");
    }

    private void ChaseTarget()
    {
        if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent cannot navigate because it is not active or not on NavMesh.");
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        // Method can be called even while not in play mode and during this time, enemy is null. Add check.
        if (enemy != null)
        {
            Gizmos.DrawWireSphere(transform.position, enemy.AttackRadius);
        }
        else
        {
            Debug.LogWarning("Gizmo is not drawn for Enemy Prefab because enemy script component is not assigned.");
        }
    }
}
