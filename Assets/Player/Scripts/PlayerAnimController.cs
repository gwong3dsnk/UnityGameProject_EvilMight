using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimController : MonoBehaviour
{
    private Animator animator;
    public event EventHandler OnAnimFXPlay;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() 
    {
        // NOTE: THen HandleAbilityPlayAnim is invokved, it is observed by both SK_ChracterHands playerAnimController component, AND SK_SmallHands_Close
        // playerAnimController.  THis causes the "Parameter 'fingerShotTrigger' does not exist." warning in the console.  
        AbilitiesManager.AbilityManagerInstance.HandleAbilityPlayAnim += DetermineAbilityName;
    }

    private void OnDisable() 
    {
        AbilitiesManager.AbilityManagerInstance.HandleAbilityPlayAnim -= DetermineAbilityName;
    }

    public void ProcessGetHitAnim()
    {
        SetAnimTrigger("getHitTrigger");
    }

    public void ProcessDeathAnim()
    {
        SetAnimTrigger("deathTrigger");
    }

    private void DetermineAbilityName(AbilityBase ability)
    {
        Logger.Log("[PlayerAnimController] - HandleAbilityPlayAnim heard in PlayerAnimController.DetermineAbilityName", this);

        if (ability == null)
        {
            Logger.LogError("[PlayerAnimController] - No PlayerAbilities found passed in.", this);
        }
        else
        {
            Logger.Log($"[PlayerAnimController] - Incoming Ability Name for AnimPlay - {ability.AbilityName}", this);
            switch (ability.AbilityName)
            {
                case AbilityNames.FingerFlick:
                    SetAnimTrigger("fingerFlickTrigger");
                    break;
                case AbilityNames.FingerShot:
                    SetAnimTrigger("fingerShotTrigger");
                    break;
                case AbilityNames.FistSlam:
                    SetAnimTrigger("fistSlamTrigger");
                    break;
            }

            Logger.Log($"[PlayerAnimController] - Finished setting animation trigger/bool.", this);
        }
    }

    private void SetAnimTrigger(string triggerName) 
    {
        Logger.Log($"[PlayerAnimController] - Setting animation trigger for {triggerName}", this);
        animator.SetTrigger(triggerName);
    }

    private void SetAnimBool(string boolName)
    {
        Logger.Log($"[PlayerAnimController] - Setting animation bool for {boolName}", this);
        animator.SetBool(boolName, true);
    }

    private void InvokeOnAnimFXPlay()
    {
        // Called by animation events.
        Logger.Log("[PlayerAnimController] - AnimEvent Triggered.  Invoking OnAnimFXPlay.", this);
        OnAnimFXPlay?.Invoke(this, EventArgs.Empty);
    }
}
