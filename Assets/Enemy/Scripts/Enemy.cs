using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] EnemyClass enemyClass;
    [SerializeField] EnemyDifficulty difficulty;
    // Unserialize below later, leave for testing and visibility
    [SerializeField] int hitPoints;
    [SerializeField] int defense;
    [SerializeField] int attack;
    [SerializeField] int spellAttack;
    [SerializeField] float attackRadius;
    public float AttackRadius { get { return attackRadius; } }
    [SerializeField] float movementSpeed;
    public float MovementSpeed { get { return movementSpeed; } }

    public void SetClassAndDifficulty(EnemyClass enemyClass, EnemyDifficulty enemyDifficulty)
    {
        this.enemyClass = enemyClass;
        this.difficulty = enemyDifficulty;
        InitializeAttributes();
    }

    private void InitializeAttributes()
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyData is not assigned.", this);
        }
        else
        {
            foreach (var stats in enemyData.enemyStatsArray)
            {
                if (stats.enemyClass == this.enemyClass && stats.difficulty == this.difficulty)
                {
                    Debug.Log("Enemy data found");
                    this.hitPoints = stats.hp;
                    this.defense = stats.def;
                    this.attack = stats.atk;
                    this.spellAttack = stats.spellAtk;
                    this.attackRadius = stats.atkRadius;
                    this.movementSpeed = stats.movementSpeed;
                    break;
                }
            }
        }
    }
}
