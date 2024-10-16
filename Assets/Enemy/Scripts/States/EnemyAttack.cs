using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] ParticleSystem primaryFX;
    [SerializeField] ParticleSystem impactFX;
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
        primaryFX.Play();
    }

    public void StopAttackFX()
    {
        if (primaryFX.isPlaying)
        {
            primaryFX.Stop();
        }        
    }
}
