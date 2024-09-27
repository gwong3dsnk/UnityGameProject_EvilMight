using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] EnemyClass enemyClass;
    public EnemyClass EnemyClass { get { return enemyClass; } }
    [SerializeField] EnemyDifficulty enemyDifficulty;
    public EnemyDifficulty EnemyDifficulty { get { return enemyDifficulty; } }
    // For testing and visibility
    [SerializeField] int t_hitPoints;
    public int HitPoints => t_hitPoints;
    [SerializeField] int t_attack;
    public int Attack { get { return t_attack; } }
    [SerializeField] float t_attackRadius;
    public float AttackRadius { get { return t_attackRadius; } }
    [SerializeField] float t_movementSpeed;
    public float MovementSpeed { get { return t_movementSpeed; } }
    [SerializeField] int t_experience;
    public int Experience => t_experience;

    public void SetClassAndDifficulty(EnemyClass enemyClass, EnemyDifficulty enemyDifficulty)
    {
        this.enemyClass = enemyClass;
        this.enemyDifficulty = enemyDifficulty;
        InitializeAttributes();
    }

    private void InitializeAttributes()
    {
        if (enemyData == null)
        {
            Logger.LogError("EnemyData is not assigned.", this);
        }
        else
        {
            foreach (var stats in enemyData.enemyStatsArray)
            {
                if (stats.enemyClass == this.enemyClass && stats.difficulty == this.enemyDifficulty)
                {
                    this.t_hitPoints = stats.hp;
                    this.t_attack = stats.atk;
                    this.t_attackRadius = stats.atkRadius;
                    this.t_movementSpeed = stats.movementSpeed;
                    this.t_experience = stats.experience;
                    break;
                }
            }
        }
    }
}
