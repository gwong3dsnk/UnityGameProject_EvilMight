using System;
using System.Collections;
using UnityEngine;

public class PlayerLevelingManager : MonoBehaviour
{
    #region Fields and Properties
    // SerializedFields
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField]
    [Tooltip("Delay in seconds to wait before enabling ability choice canvas UI.")] 
    private float onLevelUpDelay = 1.0f;

    // Serialized for testing purposes only
    [SerializeField] private int levelXPThreshold;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int excessXP;

    // Event
    public event EventHandler OnLevelUp;

    // Debug
    #if UNITY_EDITOR
    [Header("DEBUG")]
    [SerializeField] private int expOverride = 0;
    #endif
    #endregion


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

        #if UNITY_EDITOR
        if (expOverride > 0)
        {
            levelXPThreshold = expOverride;
        }
        #endif

        if (currentXP >= levelXPThreshold)
        {
            CalculateExcessXP();
            LevelUp();
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
        StartCoroutine(DelayInvokingOnLevelUpEvent());
    }

    private IEnumerator DelayInvokingOnLevelUpEvent()
    {
        yield return new WaitForSeconds(onLevelUpDelay);
        OnLevelUp?.Invoke(this, EventArgs.Empty);
    }
}
