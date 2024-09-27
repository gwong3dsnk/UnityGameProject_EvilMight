using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    private Animator animator;
    private EnemyAnimState currentState;

    private void Awake() 
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Logger.LogError("Animator or Enemy script component is missing", this);
            return;            
        }
    }

    public void DetermineEnemyClassAndAction(EnemyAnimCategory animType)
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        if (enemy == null)
        {
            Logger.LogError("Enemy script component is missing", this);
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
                        SetGetHitTrigger();
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
                        SetGetHitTrigger();
                    }            
                    break;
                default:
                    return;
            }
        }
    }

    private void SetRangeMoveBool()
    {
        animator.SetBool("isRangeAttacking", false);
        animator.SetBool("isWalkingForward", true);
    }

    private void SetRangeAttackBool()
    {
        animator.SetBool("isWalkingForward", false);
        animator.SetBool("isRangeAttacking", true);
    }

    private void SetMeleeMoveBool()
    {
        animator.SetBool("isMeleeAttacking", false);
        animator.SetBool("isRunningForward", true);
    }

    private void SetMeleeAttackBool()
    {
        animator.SetBool("isRunningForward", false);
        animator.SetBool("isMeleeAttacking", true);
    }

    private void SetGetHitTrigger()
    {
        animator.SetTrigger("GetHitTrigger");
    }

    public void ReceiveAttackAnimEvent()
    {
        Logger.Log("ReceiveAttackAnimEvent triggered by enemy Attack01 anim event.", this);
        EnemyAttack enemyAttack = GetComponentInParent<EnemyAttack>();
        if (enemyAttack != null)
        {
            enemyAttack.PlayAttackFX();
        }
        else
        {
            Logger.LogError("Missing reference to EnemyAttack.", this);
            return;
        }
    }

    public void OnGetHitCompletion()
    {
        Logger.Log("Processing logic in OnGetHitCompletion", this);
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
