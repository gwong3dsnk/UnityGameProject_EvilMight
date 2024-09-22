using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimController : MonoBehaviour
{
    private Animator animator;

    private void Start() 
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() 
    {
        PlayerAbilitiesManager.AbilityManagerInstance.OnActivationCompletion += DetermineAbility;
    }

    private void OnDisable() 
    {
        PlayerAbilitiesManager.AbilityManagerInstance.OnActivationCompletion -= DetermineAbility;
    }

    public void DetermineAbility(object sender, System.EventArgs e)
    {
        Logger.Log("OnActivationComplete heard", this);
        PlayerAbilitiesManager abilityManager = sender as PlayerAbilitiesManager;
        if (abilityManager == null)
        {
            Logger.LogError("No reference to PlayerAbilitiesManager as sender is null.", this);
        }
        else
        {
            if (abilityManager.ActiveAbilityForAnim == null)
            {
                Logger.Log("No data found for ActiveAbilityForAnim property", this);
            }
            else
            {
                Logger.LogError($"OnPlayerAbilityActivation EVENT - {abilityManager.ActiveAbilityForAnim.AbilityName}");
            }
        }
    }

    private void SetAnimTrigger()
    {
        
    }

    private void SetAnimBool()
    {

    }

    // private void SetAnimationTriggerToPlay(object sender, System.EventArgs e)
    // {
    //     Logger.Log("Event WithinFingerFlickRange heard.  Running SetAnimationTrigger", this);
    //     animator.SetTrigger("fingerFlickBool");
    // }
}
