using System;
using NaughtyAttributes;
using UnityEngine;

public abstract class HealthManagement : MonoBehaviour
{
    [SerializeField] [ReadOnly] protected float currentHealth;
    public float CurrentHealth => currentHealth;
    public event EventHandler OnDeath;
    protected int maxHealth = 1;

    protected abstract void OnParticleCollision(GameObject other);
    protected abstract void HandleDeath();
    protected abstract void HandleStillAlive();

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            HandleDeath(); 
        }
        else
        {
            HandleStillAlive();
        }
    }

    protected void InvokeOnDeathEvent()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }
}
