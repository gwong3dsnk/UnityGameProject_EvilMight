using System;
using UnityEngine;

public abstract class HealthManagement : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 1;
    public float CurrentHealth => currentHealth;
    public event EventHandler OnDeath;
    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            BeginDeathSequence(); 
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

    protected abstract void BeginDeathSequence();

    protected abstract void HandleStillAlive();
}
