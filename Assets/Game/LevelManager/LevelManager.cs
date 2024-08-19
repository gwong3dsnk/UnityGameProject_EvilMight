using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Attach this script as a component to an empty gameobject in the Player gameobject
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Canvas abilityCardCanvas;
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

        Time.timeScale = 0; // Restore time once player chooses an ability card.
        abilityCardCanvas.gameObject.SetActive(true);
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
