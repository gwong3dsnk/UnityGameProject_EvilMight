using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] AbilityCardChooserUI abilitychoiceUI;
    [SerializeField] AbilityCardGenerator cardGenerator;
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
            Debug.LogError("Missing reference to PlayerHealth script on LevelSystem object", this);
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
        Debug.Log("Leveling Up");
        OnLevelUp?.Invoke(this, EventArgs.Empty);
        currentLevel += 1;
        playerHealth.ResetHealth();
        CalculateXPThreshold();

        if (excessXP > 0)
        {
            currentXP = excessXP;
        }
        else
        {
            currentXP = 0;
        }
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
