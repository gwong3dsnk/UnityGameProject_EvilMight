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
    private EnemyAnimController enemyAnimController;
    private float distanceToTarget = Mathf.Infinity;
    private Vector3 lastPosition;
    private Coroutine updatePositionCoroutine;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        enemyAnimController = GetComponentInChildren<EnemyAnimController>();

        if (enemyAnimController == null || enemy == null)
        {
            Logger.LogError("EnemyAnimController or Enemy script component is missing", this);
            return;
        }
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        enemyAnimController.Initialize(enemy);

        if (player == null)
        {
            Logger.LogError("PlayerMovement script is not found in the scene.", this);
            return;
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
            if (GridManager.GridManagerInstance != null && lastPosition != transform.position)
            {
                GridManager.GridManagerInstance.UpdatePosition(GetComponent<Collider>());
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

        if (distanceToTarget <= navMeshAgent.stoppingDistance) // player within range, enemy will attack
        {
            EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
            
            if (enemyAttack != null)
            {
                enemyAttack.AttackTarget();
            }
            else
            {
                Logger.LogError("Missing EnemyAttack script component.", this);
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
            // Check if enemy is ranged or melee.  Ranged will walk, melee will run.
            enemyAnimController.DetermineEnemyClassAndAction(EnemyAnimType.Movement);
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
            Logger.LogWarning("Gizmo is not drawn for Enemy Prefab because enemy script component is not assigned.");
        }
    }
}
