using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthManagement : MonoBehaviour
{
    public int CurrentHealth => currentHealth;

    [SerializeField] protected int maxHealth = 1;
    protected int currentHealth;

    public event EventHandler OnDeath;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    protected void TriggerDeathEvent()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    protected abstract void HandleDeath();
}
