using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    // Unserialize below later.
    [SerializeField] private int levelXPThreshold;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int excessXP;
    public event EventHandler OnLevelUp;

    private void Awake()
    {
        CalculateXPThreshold();
    }

    private void Start()
    {
        if (playerHealth == null)
        {
            Logger.LogError("Missing reference to PlayerHealth script on LevelSystem object", this);
        }
    }
    
    public void AddXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= levelXPThreshold)
        {
            CalculateExcessXP();
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Logger.Log($"Player is leveling from {currentLevel} to {currentLevel += 1}", this);
        currentLevel += 1;
        playerHealth.ResetHealth();
        CalculateXPThreshold();
        currentXP = excessXP > 0 ? excessXP : 0;
        OnLevelUp?.Invoke(this, EventArgs.Empty);
    }

    private void CalculateXPThreshold()
    {
        levelXPThreshold = (currentLevel * 10) - 5;
    }

    private void CalculateExcessXP()
    {
        excessXP = currentXP - levelXPThreshold;
    }
}
