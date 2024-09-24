using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimController : MonoBehaviour
{
    private Animator animator;
    private PlayerAbilities activeAbility;
    public PlayerAbilities ActiveAbility => activeAbility;
    public event EventHandler OnAnimFXPlay;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() 
    {
        PlayerAbilitiesManager.AbilityManagerInstance.OnAbilityActivationCompletion += DetermineAbilityName;
    }

    private void OnDisable() 
    {
        PlayerAbilitiesManager.AbilityManagerInstance.OnAbilityActivationCompletion -= DetermineAbilityName;
    }

    public void DetermineAbilityName(object sender, System.EventArgs e)
    {
        Logger.Log("OnActivationCompletion heard in PlayerAnimController.DetermineAbilityName", this);
        PlayerAbilitiesManager abilityManager = sender as PlayerAbilitiesManager;
        if (abilityManager == null)
        {
            Logger.LogError("No reference to PlayerAbilitiesManager as sender is null.", this);
        }
        else
        {
            if (abilityManager.ActiveAbilityForAnim == null)
            {
                Logger.LogError("No data found for ActiveAbilityForAnim property", this);
            }
            else
            {
                Logger.Log($"Incoming Ability Name for AnimPlay - {abilityManager.ActiveAbilityForAnim.AbilityName}", this);
                activeAbility = abilityManager.ActiveAbilityForAnim;
                switch (activeAbility.AbilityName)
                {
                    case AbilityNames.FingerFlick:
                        SetAnimTrigger("fingerFlickTrigger");
                        break;
                    case AbilityNames.FingerShot:
                        SetAnimBool("fingerShotBool");
                        break;
                    case AbilityNames.FistSlam:
                        SetAnimTrigger("fistSlamTrigger");
                        break;
                }

                Logger.Log($"Finished setting animation trigger/bool.", this);
            }
        }
    }

    private void SetAnimTrigger(string triggerName)
    {
        Logger.Log($"Setting animation trigger for {triggerName}", this);
        animator.SetTrigger(triggerName);
    }

    private void SetAnimBool(string boolName)
    {
        Logger.Log($"Setting animation bool for {boolName}", this);
        animator.SetBool(boolName, true);
    }

    private void InvokeOnAnimFXPlay()
    {
        Logger.Log("Invoking OnAnimFXPlay in PlayerAnimController to play FX.", this);
        OnAnimFXPlay?.Invoke(this, EventArgs.Empty);
    }
}
