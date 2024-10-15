using NaughtyAttributes;
using System.Linq;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    #region Fields and Properties
    [SerializeField] protected EnemyData enemyData;

    // ReadOnly SerializedFields
    [SerializeField] [ReadOnly] protected EnemyClass enemyClass;
    [SerializeField] [ReadOnly] protected EnemyDifficulty enemyDifficulty;
    [SerializeField] [ReadOnly] protected int hitPoints;
    [SerializeField] [ReadOnly] protected int attack;
    [SerializeField] [ReadOnly] protected float attackRadius;
    [SerializeField] [ReadOnly] protected float movementSpeed;
    [SerializeField] [ReadOnly] protected int experience;  
    protected GameObject prefab;  

    // Public Properties
    public EnemyClass EnemyClass => enemyClass;
    public EnemyDifficulty EnemyDifficulty => enemyDifficulty;
    public int HitPoints => hitPoints;
    public int Attack => attack;
    public float AttackRadius => attackRadius;
    public float MovementSpeed => movementSpeed;
    public int Experience => experience;
    public GameObject Prefab => prefab;
    #endregion

    public virtual void SetClassAndDifficulty(EnemyClass enemyClass, EnemyDifficulty enemyDifficulty)
    {
        this.enemyClass = enemyClass;
        this.enemyDifficulty = enemyDifficulty;
        InitializeAttributes();
    }

    protected virtual void InitializeAttributes()
    {
        if (enemyData == null)
        {
            Logger.LogError("EnemyData is not assigned.", this);
        }
        else
        {
            EnemyData.EnemyStats enemyStats = enemyData.enemyStatsArray.FirstOrDefault
            (
                stats => stats.enemyClass == this.enemyClass && stats.difficulty == this.enemyDifficulty
            );

            if (enemyStats != null)
            {
                this.hitPoints = enemyStats.hp;
                this.attack = enemyStats.atk;
                this.attackRadius = enemyStats.atkRadius;
                this.movementSpeed = enemyStats.movementSpeed;
                this.experience = enemyStats.experience;
                this.prefab = enemyStats.prefab;
            }
            else
            {
                Logger.LogError($"No matching enemy data found in EnemyData based on {this.enemyClass} and {this.enemyDifficulty}.", this);
            }
        }
    }
}
