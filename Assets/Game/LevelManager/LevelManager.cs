using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] AbilityCardChooserUI abilitychoiceUI;
    // Unserialize below later.
    [SerializeField] private int levelXPThreshold;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int excessXP;

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

        // TO THINK ABOUT: Make this into an event so that when a player levels up, the event is fired, method below is called.
        abilitychoiceUI.EnableAbilityChoiceCanvas();
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
