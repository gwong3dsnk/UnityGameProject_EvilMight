using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    private Animator animator;
    private EnemyAnimState currentState;
    private EnemyHealth enemyHealth;
    private const string getHitTrigger = "GetHitTrigger";
    private const string rangeAttacking = "isRangeAttacking";
    private const string deathTrigger = "DeathTrigger";
    private const string walkingForward = "isWalkingForward";
    private const string meleeAttacking = "isMeleeAttacking";
    private const string runningForward = "isRunningForward";

    private void Awake() 
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        if (animator == null || enemyHealth == null)
        {
            Logger.LogError("[EnemyAnimController] - Animator or EnemyHealth reference is missing", this);
            return;            
        }
    }

    private void OnEnable()
    {
        enemyHealth.OnDeath += ProcessEnemyDeath;        
    }

    private void OnDisable()
    {
        enemyHealth.OnDeath -= ProcessEnemyDeath;
    }

    public void DetermineEnemyClassAndAction(EnemyAnimCategory animType)
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        if (enemy == null)
        {
            Logger.LogError($"[{this.name}] - Enemy script component is missing", this);
            return;            
        }
        else
        {
            switch (enemy.EnemyClass)
            {
                case EnemyClass.Range:
                    if (animType == EnemyAnimCategory.Movement)
                    {
                        SetRangeMoveBool();
                        currentState = EnemyAnimState.RangeMovement;
                    }
                    else if (animType == EnemyAnimCategory.Attack)
                    {
                        SetRangeAttackBool();
                        currentState = EnemyAnimState.RangeAttack;
                    }
                    else
                    {
                        SetTriggerParam(getHitTrigger);
                    }
                    break;
                case EnemyClass.Melee:
                    if (animType == EnemyAnimCategory.Movement)
                    {
                        SetMeleeMoveBool();
                        currentState = EnemyAnimState.MeleeMovement;
                    }
                    else if (animType == EnemyAnimCategory.Attack)
                    {
                        SetMeleeAttackBool();
                        currentState = EnemyAnimState.MeleeAttack;
                    }
                    else
                    {
                        SetTriggerParam(getHitTrigger);
                    }            
                    break;
                default:
                    return;
            }
        }
    }

    private void SetRangeMoveBool()
    {
        animator.SetBool(rangeAttacking, false);
        animator.SetBool(walkingForward, true);
    }

    private void SetRangeAttackBool()
    {
        animator.SetBool(walkingForward, false);
        animator.SetBool(rangeAttacking, true);
    }

    private void SetMeleeMoveBool()
    {
        animator.SetBool(meleeAttacking, false);
        animator.SetBool(runningForward, true);
    }

    private void SetMeleeAttackBool()
    {
        animator.SetBool(runningForward, false);
        animator.SetBool(meleeAttacking, true);
    }

    private void ProcessEnemyDeath(object sender, System.EventArgs e)
    {
        SetTriggerParam(deathTrigger);
    }

    private void SetTriggerParam(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void ReceiveAttackAnimEvent()
    {
        EnemyAttack enemyAttack = GetComponentInParent<EnemyAttack>();
        if (enemyAttack != null)
        {
            enemyAttack.PlayAttackFX();
        }
        else
        {
            Logger.LogError($"[{this.name}] - Missing reference to EnemyAttack.", this);
            return;
        }
    }

    public void OnGetHitCompletion()
    {
        switch (currentState)
        {
            case EnemyAnimState.RangeMovement:
                SetRangeMoveBool();
                break;
            case EnemyAnimState.RangeAttack:
                SetRangeAttackBool();
                break;
            case EnemyAnimState.MeleeMovement:
                SetMeleeMoveBool();
                break;
            case EnemyAnimState.MeleeAttack:
                SetMeleeAttackBool();
                break;
        }
    }
}
