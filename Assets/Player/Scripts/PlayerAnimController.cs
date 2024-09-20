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
        animator.SetTrigger("fingerShotTrigger"); // Should be replaced by meleeSlash when ready to changed default ability.
    }

    private void OnEnable() 
    {
        PlayerAbilitiesManager.AbilityManagerInstance.OnActivationCompletion += DetermineAbility;
    }

    private void OnDisable() 
    {
        PlayerAbilitiesManager.AbilityManagerInstance.OnActivationCompletion -= DetermineAbility;
    }

    private void DetermineAbility(object sender, System.EventArgs e)
    {
        Logger.Log("OnActivationComplete heard", this);
        PlayerAbilitiesManager abilityManager = sender as PlayerAbilitiesManager;
        if (abilityManager == null)
        {
            Logger.LogError("No reference to PlayerAbilitiesManager as sender is null.", this);
        }
        else
        {
            Logger.LogError($"OnPlayerAbilityActivation EVENT - {abilityManager.ActiveAbilityForAnim.AbilityName}");
        }
    }
}
