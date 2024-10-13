using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimController : MonoBehaviour
{
    public event Action<string> OnAnimFXPlay;
    private Animator animator;
    private const string getHitTrigger = "getHitTrigger";
    private const string deathTrigger = "deathTrigger";
    private const string fingerFlickTrigger = "fingerFlickTrigger";
    private const string fingerShotTrigger = "fingerShotTrigger";
    private const string fistSlamTrigger = "fistSlamTrigger";            

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

    #region Public Methods
    public void ProcessGetHitAnim()
    {
        SetAnimTrigger(getHitTrigger);
    }

    public void ProcessDeathAnim()
    {
        SetAnimTrigger(deathTrigger);
    }
    #endregion

    #region Private Methods
    private void DetermineAbilityName(AbilityBase ability)
    {
        if (ability == null)
        {
            Logger.LogError("[PlayerAnimController] - No PlayerAbilities found passed in.", this);
        }
        else
        {
            switch (ability.AbilityName)
            {
                case AbilityNames.FingerFlick:
                    SetAnimTrigger(fingerFlickTrigger);
                    break;
                case AbilityNames.FingerShot:
                    SetAnimTrigger(fingerShotTrigger);
                    break;
                case AbilityNames.FistSlam:
                    SetAnimTrigger(fistSlamTrigger);
                    break;
            }
        }
    }

    private void SetAnimTrigger(string triggerName) 
    {
        foreach (var param in animator.parameters)
        {
            if (param.name == triggerName)
            {
                animator.SetTrigger(triggerName);
                return;
            }
        }
    }

    private void InvokeOnAnimFXPlay()
    {
        // Called by animation events.
        string animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        OnAnimFXPlay?.Invoke(animationName);
    }
    #endregion
}
