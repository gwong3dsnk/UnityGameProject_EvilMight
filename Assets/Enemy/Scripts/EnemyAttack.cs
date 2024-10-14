using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] ParticleSystem attackFX;
    private EnemyAnimController enemyAnimController;

    private void Awake() 
    {
        enemyAnimController = GetComponentInChildren<EnemyAnimController>();

        if (enemyAnimController == null)
        {
            Logger.LogError("EnemyAnimController script component is missing", this);
            return;
        }
    }

    public void AttackTarget()
    {
        enemyAnimController.DetermineEnemyClassAndAction(EnemyAnimCategory.Attack);
    }

    public void PlayAttackFX()
    {
        StopAttackFX();
        attackFX.Play();
    }

    public void StopAttackFX()
    {
        if (attackFX.isPlaying)
        {
            attackFX.Stop();
        }        
    }
}
