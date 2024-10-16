using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    #region Fields and Properties
    // SerializedFields
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float positionUpdateInterval = 0.5f;

    // Private Fields
    private NavMeshAgent navMeshAgent;
    private Enemy enemy;
    private EnemyHealth enemyHealth;
    private EnemyAttack enemyAttack;
    private EnemyAnimController enemyAnimController;
    private float distanceToTarget = Mathf.Infinity;
    private Vector3 lastPosition;
    private Coroutine updatePositionCoroutine;
    private bool isEnemyDead = false;
    #endregion

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyAnimController = GetComponentInChildren<EnemyAnimController>();

        if (enemyAnimController == null || enemy == null)
        {
            Logger.LogError($"[{this.name}] - EnemyAnimController or Enemy component is missing.", this);
            return;
        }

        if (navMeshAgent == null)
        {
            Logger.LogError($"[{this.name}] - NavMeshAgent component is missing", this);
            return;
        }        
        
        if (enemyAttack == null || enemyHealth == null)
        {
            Logger.LogError($"[{this.name}] - Missing EnemyAttack or EnemyHealth script component.", this);
            return;
        }        
    }

    private void Start()
    {
        lastPosition = transform.position;
        updatePositionCoroutine = StartCoroutine(UpdatePositionPeriodically());
    }

    private void OnEnable()
    {
        enemyHealth.OnDeath += StopMovement;
    }

    private void Update()
    {
        if (!isEnemyDead)
        {
            distanceToTarget = Vector3.Distance(enemy.PlayerHealth.transform.position, transform.position);
            MoveToPlayer();
        }
    }

    private void OnDisable() 
    {
        enemyHealth.OnDeath -= StopMovement;

        if (updatePositionCoroutine != null)
        {
            StopCoroutine(updatePositionCoroutine);
        }
    }  

    private void StopMovement(object sender, System.EventArgs e)
    {
        if (sender as EnemyHealth)
        {
            isEnemyDead = true;
        }
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

    private void MoveToPlayer()
    {
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
            navMeshAgent.velocity = Vector3.zero;
            enemyAttack.AttackTarget();
        }        
    }

    private void FacePlayer()
    {
        // Rotate the enemy to face the player
        Vector3 direction = (enemy.PlayerHealth.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void ChaseTarget()
    {
        if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(enemy.PlayerHealth.transform.position);
            // Check if enemy is ranged or melee.  Ranged will walk, melee will run.
            enemyAnimController.DetermineEnemyClassAndAction(EnemyAnimCategory.Movement);
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
    }
}
