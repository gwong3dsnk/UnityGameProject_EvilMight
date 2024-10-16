using System;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    public event EventHandler OnEnemyAttackParticleAnimEvent;
    public event EventHandler OnEnemyAttackPhysicalAnimEvent;
    private Animator animator;
    private EnemyAnimState currentState;
    private EnemyHealth enemyHealth;
    private Enemy enemy;
    private const string getHitTrigger = "GetHitTrigger";
    private const string rangeAttacking = "isRangeAttacking";
    private const string deathTrigger = "DeathTrigger";
    private const string walkingForward = "isWalkingForward";
    private const string meleeAttacking = "isMeleeAttacking";
    private const string runningForward = "isRunningForward";

    private void Awake() 
    {
        enemy = GetComponentInParent<Enemy>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        if (animator == null || enemyHealth == null)
        {
            Logger.LogError("[EnemyAnimController] - Animator or EnemyHealth reference is missing", this);
            return;            
        }

        if (enemy == null)
        {
            Logger.LogError("[EnemyAnimController] - Enemy reference is missing", this);
            return;
        }
    }

    private void OnEnable()
    {
        enemyHealth.OnDeath += PlayEnemyDeathAnim;        
    }

    private void OnDisable()
    {
        enemyHealth.OnDeath -= PlayEnemyDeathAnim;
    }

    public void DetermineEnemyClassAndAction(EnemyAnimCategory animType)
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

    public void ReceiveAttackParticleAnimEvent()
    {
        // Method called by range enemy attack anim event
        OnEnemyAttackParticleAnimEvent?.Invoke(this, EventArgs.Empty);
    }

    public void ReceiveAttackPhysicalAnimEvent()
    {
        // Method called by melee enemy attack anim event
        OnEnemyAttackPhysicalAnimEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnGetHitCompletion()
    {
        // Method called by GetHit anim event
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

    private void PlayEnemyDeathAnim(object sender, System.EventArgs e)
    {
        SetTriggerParam(deathTrigger);
    }

    private void SetTriggerParam(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
}
