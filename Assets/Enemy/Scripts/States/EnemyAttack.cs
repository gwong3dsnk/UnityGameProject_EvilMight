using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] ParticleSystem primaryFX;
    [SerializeField] ParticleSystem impactFX;
    private Enemy enemy;
    private EnemyAnimController enemyAnimController;

    private void Awake() 
    {
        enemy = GetComponent<Enemy>();
        enemyAnimController = GetComponentInChildren<EnemyAnimController>();

        if (enemyAnimController == null || enemy == null)
        {
            Logger.LogError($"[{this.name}] - EnemyAnimController or Enemy script component is missing", this);
            return;
        }
    }

    private void OnEnable()
    {
        enemyAnimController.OnEnemyAttackParticleAnimEvent += ProcessParticleAttack;
        enemyAnimController.OnEnemyAttackPhysicalAnimEvent += ProcessPhysicalAttack;
    }

    private void OnDisable()
    {
        enemyAnimController.OnEnemyAttackParticleAnimEvent -= ProcessParticleAttack;
        enemyAnimController.OnEnemyAttackPhysicalAnimEvent -= ProcessPhysicalAttack;
    }    

    public void AttackTarget()
    {
        enemyAnimController.DetermineEnemyClassAndAction(EnemyAnimCategory.Attack);
    }

    private void ProcessParticleAttack(object sender, System.EventArgs e)
    {
        PlayPrimaryFX();
        // PlayImpactFX();
    }

    private void PlayPrimaryFX()
    {
        if (primaryFX.isPlaying)
        {
            primaryFX.Stop();
        }       

        primaryFX.Play();
    }

    private void PlayImpactFX()
    {
        if (impactFX.isPlaying)
        {
            impactFX.Stop();
        }

        impactFX.Play();
    }    

    private void ProcessPhysicalAttack(object sender, System.EventArgs e)
    {
        SetImpactFXLocation();
        PlayImpactFX();
        enemy.PlayerHealth.TakePhysicalDamage(enemy.Attack);
    }

    private void SetImpactFXLocation()
    {
        impactFX.transform.position = enemy.PlayerHealth.transform.position;
    }
}
