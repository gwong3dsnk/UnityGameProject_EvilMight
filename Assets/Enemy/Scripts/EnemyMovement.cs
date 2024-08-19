using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] float positionUpdateInterval = 0.5f;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Enemy enemy;
    private float distanceToTarget = Mathf.Infinity;
    private GridManager gridManager;
    private Vector3 lastPosition;
    private Coroutine updatePositionCoroutine;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();

        if (navMeshAgent == null || enemy == null)
        {
            Debug.LogError("NavMeshAgent or Enemy script component is missing", this);
        }
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;

        if (player == null)
        {
            Debug.LogError("PlayerMovement script is not found in the scene.", this);
        }

        if (GridManager.GridManagerInstance != null)
        {
            gridManager = GridManager.GridManagerInstance;
        }
        else
        {
            Debug.LogError("GridManager Instance is not found in the scene.", this);
        }

        lastPosition = transform.position;

        updatePositionCoroutine = StartCoroutine(UpdatePositionPeriodically());
    }

    private IEnumerator UpdatePositionPeriodically()
    {
        // While enemy is active, Update it's position in the grid partitioning system after every interval.
        while (true)
        {
            yield return new WaitForSeconds(positionUpdateInterval);

            // Update the grid if the position has changed
            if (gridManager != null && lastPosition != transform.position)
            {
                gridManager.UpdatePosition(GetComponent<Collider>());
                lastPosition = transform.position;
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            distanceToTarget = Vector3.Distance(player.position, transform.position);
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        if (navMeshAgent == null) return;

        FacePlayer();

        // Set the enemy traversal speed and stopping distance
        navMeshAgent.speed = enemy.MovementSpeed;
        navMeshAgent.stoppingDistance = enemy.AttackRadius;

        if (distanceToTarget >= navMeshAgent.stoppingDistance) // enemy will chase the player
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance) // enemy player within range, will attack
        {
            EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
            
            if (enemyAttack != null)
            {
                enemyAttack.AttackTarget(enemy);
            }
            else
            {
                Debug.LogError("Missing EnemyAttack script component.", this);
            }
        }        
    }

    private void FacePlayer()
    {
        // Rotate the enemy to face the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void ChaseTarget()
    {
        if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    private void OnDisable() 
    {
        // On enemy death and subsequent deactivation, stop the coroutine from executing.
        if (updatePositionCoroutine != null)
        {
            StopCoroutine(updatePositionCoroutine);
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
