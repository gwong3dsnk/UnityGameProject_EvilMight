using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    private Animator animator;
    private Enemy enemy;

    private void Awake() 
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Logger.LogError("Animator script component is missing", this);
            return;            
        }
    }

    public void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void DetermineEnemyClassAndAction(EnemyAnimType animType)
    {
        switch (this.enemy.EnemyClass)
        {
            case EnemyClass.Range:
                if (animType == EnemyAnimType.Movement)
                {
                    SetRangeWalkBool();
                }
                else if (animType == EnemyAnimType.Attack)
                {
                    SetRangeAttackBool();
                }
                else
                {
                    SetGetHitTrigger();
                }
                break;
            case EnemyClass.Melee:
                if (animType == EnemyAnimType.Movement)
                {
                    SetMeleeRunBool();
                }
                else if (animType == EnemyAnimType.Attack)
                {
                    SetMeleeAttackBool();
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

    private void SetRangeWalkBool()
    {
        animator.SetBool("isRangeAttacking", false);
        animator.SetBool("isWalkingForward", true);
    }

    private void SetRangeAttackBool()
    {
        // Method call by Attack01Range in EnemyAnimController
        animator.SetBool("isWalkingForward", false);
        animator.SetBool("isRangeAttacking", true);
    }

    private void SetMeleeRunBool()
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

    }

    public void ReceiveAttackAnimEvent()
    {
        Logger.Log("Receiving anim event.", this);
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
}
