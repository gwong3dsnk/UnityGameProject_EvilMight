using UnityEngine;

public class PlayerHealth : HealthManagement
{
    [SerializeField] int playerHealth = 3;
    private Enemy enemy;

    protected override void Start()
    {
        maxHealth = playerHealth;
        base.Start();
    }

    private void OnParticleCollision(GameObject other) 
    {
        Logger.Log("[PlayerHealth] - Detecting particle collision hit on player", this);
        GameObject enemyPrefab = other.transform.parent?.gameObject;
        enemy = enemyPrefab.GetComponent<Enemy>();

        if (enemy != null)
        {
            TakeDamage(enemy.Attack);
        }
    }

    protected override void HandleDeath()
    {
        GetComponent<PlayerDeathHandler>().HandleDeath();
    }

    protected override void HandleStillAlive()
    {
        Logger.Log("[PlayerHealth] - NOTE: Will handle player get hit response anim play.", this);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
