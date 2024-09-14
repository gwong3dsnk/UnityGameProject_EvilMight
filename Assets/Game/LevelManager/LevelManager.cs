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
        Logger.Log($"Current Exp (AddXP) - {currentXP}", this);
        Logger.Log($"Amount of Exp to Add - {amount}", this);
        currentXP += amount;
        Logger.Log($"New Current Exp (AddXP) - {currentXP}", this);

        if (currentXP >= levelXPThreshold)
        {
            CalculateExcessXP();
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Logger.Log("------------------------------------------------", this);
        Logger.Log($"PLAYER LEVEL UP DETECTED", this);
        Logger.Log($"Current Level - {currentLevel}", this);
        currentLevel += 1;
        Logger.Log($"New Level - {currentLevel}", this);
        playerHealth.ResetHealth();
        CalculateXPThreshold();
        Logger.Log($"Current Exp (LevelUp) - {currentXP}", this);
        currentXP = excessXP > 0 ? excessXP : 0;
        Logger.Log($"New Current Exp (LevelUp) - {currentXP}", this);
        Logger.Log("Processing player level-up complete.  Freezing time.", this);
        Logger.Log("------------------------------------------------", this);
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
