using System;
using UnityEngine;

public abstract class HealthManagement : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 1;
    protected float currentHealth;
    public float CurrentHealth => currentHealth;
    public event EventHandler OnDeath;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            BeginPlayerDeathSequence();
        }
        else
        {
            HandleStillAlive();
        }
    }

    protected void TriggerDeathEvent()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    protected abstract void BeginPlayerDeathSequence();

    protected abstract void HandleStillAlive();
}
