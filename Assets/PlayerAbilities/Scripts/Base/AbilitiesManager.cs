using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilitiesManager : MonoBehaviour
{
    #region Fields and Properties
    // SerializedFields
    [SerializeField] AbilityDatabaseManager abilityDatabaseManager;
    [SerializeField] ActivateButtonOnClick activateButtonOnClick;
    [SerializeField] GameObject playerDefaultAbility;
    [SerializeField] GameObject[] allAbilities;

    // Public Fields/Properties/Events
    public static AbilitiesManager AbilityManagerInstance { get; private set; }
    public event EventHandler OnAbilityActivationCompletion;
    public event Action<AbilityBase> HandleAbilityPlayAnim;
    public List<AbilityBase> ActiveAbilities => activeAbilities;

    // Private Fields
    private PlayerAnimController baseHandsAnimController;
    private PlayerAnimController smallHandsAnimController; 
    private List<AbilityBase> activeAbilities = new List<AbilityBase>();
    #endregion

    private void Awake()
    {
        Logger.Log($"[{this.name}] - Initializing OnAwake logic.", this);

        GetAnimControllerReferences();

        if (AbilityManagerInstance == null)
        {
            AbilityManagerInstance = this;
        }
        else if (AbilityManagerInstance != this)
        {
            Destroy(gameObject);
        }

        if (abilityDatabaseManager == null || activateButtonOnClick == null)
        {
            Logger.LogError($"[{this.name}] - Missing references to activateButtonOnClick or AbilityDatabaseManager.", this);
            return;
        }

        if (baseHandsAnimController == null || smallHandsAnimController == null)
        {
            Logger.LogError($"[{this.name}] - Missing either AbilityHelper script component or AbilityHelperData on PlayerAbilityContainer gameobject", this);
            return;
        }
    }

    private void OnEnable() 
    {
        activateButtonOnClick.OnAbilityChosen += BeginUnlockingNewAbility;
        baseHandsAnimController.OnAnimFXPlay += PlayAnimEventFX;
        smallHandsAnimController.OnAnimFXPlay += PlayAnimEventFX;
    }

    private void Start()
    {
        // Call this to instantiate the player's default ability right away.
        BeginUnlockingNewAbility(this, EventArgs.Empty);        
    }

    private void OnDisable()
    {
        activateButtonOnClick.OnAbilityChosen -= BeginUnlockingNewAbility;
        baseHandsAnimController.OnAnimFXPlay -= PlayAnimEventFX;
        smallHandsAnimController.OnAnimFXPlay -= PlayAnimEventFX;
    }    

    #region Public Methods
    public void AddAbility(AbilityBase ability)
    {
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            ability.ActivateAbility();
        }
    }

    /// <summary>
    /// Called by PlayerDeathHandler.PlayerDeathCoroutine when the player dies to ensure 
    /// abilities don't kill the enemy after the player has already died.
    /// </summary>
    public void DeactivatePlayerAbilities()
    {
        foreach (AbilityBase ability in activeAbilities)
        {
            ability.DeactivateAbility();
        }
    }

    public void BeginUnlockingNewAbility(object sender, System.EventArgs e)
    {
        ActivateButtonOnClick activateAbilityButtonClicked = sender as ActivateButtonOnClick;
        GameObject abilityToActivate;
        bool isPlayerDefaultAbility;

        if (activateAbilityButtonClicked != null)
        {
            abilityToActivate = allAbilities.FirstOrDefault(ability => ability.GetComponent<AbilityBase>().AbilityName == activateButtonOnClick.SelectedAbility.abilityName);
            isPlayerDefaultAbility = false;
        }
        else
        {
            abilityToActivate = allAbilities.FirstOrDefault(ability => ability.name == playerDefaultAbility.name);
            isPlayerDefaultAbility = true;
        }

        if (!abilityToActivate.activeInHierarchy)
        {
            abilityToActivate.SetActive(true);
        }

        // Process AddAbility.
        AddAbility(abilityToActivate.GetComponent<AbilityBase>());

        // Remove the unlocked ability from the ability database so it won't be shown in future level-ups.
        abilityDatabaseManager.RemoveAbilityFromDatabase(abilityToActivate.GetComponent<AbilityBase>());

        if (!isPlayerDefaultAbility)
        {
            InvokeOnActivationCompletion();
        }
    }

    public void InvokeOnActivationCompletion()
    {
        OnAbilityActivationCompletion?.Invoke(this, EventArgs.Empty); 
    }

    public void InvokeHandleAbilityPlayAnimEvent(AbilityBase ability)
    {
        HandleAbilityPlayAnim?.Invoke(ability);
    }
    #endregion

    #region Private Methods
    private void GetAnimControllerReferences()
    {
        AbilityHelperData fingerFlickHelperData =
            GetComponent<AbilityHelper>()
                .AbilityHelperData
                .FirstOrDefault(data => data.abilityNames == AbilityNames.FingerFlick);

        if (fingerFlickHelperData != null && fingerFlickHelperData.meshSockets.Length > 0)
        {
            baseHandsAnimController = fingerFlickHelperData.meshSockets[0].renderMesh.GetComponent<PlayerAnimController>();
            if (baseHandsAnimController == null)
            {
                Logger.LogError($"[{this.name}] - Unable to find PlayerAnimController component via BaseHands render mesh in AbilityHelper.", this);
            }
        }
        else
        {
            Logger.LogError($"[{this.name}] - Cannot find FingerFlick helperData in {this.transform.name}'s AbilityHelper array.", this);
        }

        AbilityHelperData fingerShotHelperData =
            GetComponent<AbilityHelper>()
                .AbilityHelperData
                .FirstOrDefault(data => data.abilityNames == AbilityNames.FingerShot);

        if (fingerShotHelperData != null && fingerShotHelperData.meshSockets.Length > 0)
        {
            smallHandsAnimController = fingerShotHelperData.meshSockets[0].renderMesh.GetComponent<PlayerAnimController>();
            if (smallHandsAnimController == null)
            {
                Logger.LogError($"[{this.name}] - Unable to find PlayerAnimController component via SmallHands render mesh in AbilityHelper.", this);
            }            
        }
        else
        {
            Logger.LogError($"[{this.name}] - Cannot find FingerShot helperData in {this.transform.name}'s AbilityHelper array.", this);
        }
    }

    /// <summary>
    /// Called by PlayerAnimController.InvokeOnAnimFXPlay().  Find ability class to play particle systems on based on passed in string.
    /// </summary>
    /// <param name="animationName"></param>
    private void PlayAnimEventFX(string animationName)
    {
        AbilityBase[] abilityBases = GetComponentsInChildren<AbilityBase>();

        foreach (AbilityBase ability in abilityBases)
        {
            if (ability.AbilityName.ToString() == animationName)
            {
                ability.HandleAnimEventFX();
            }
        }
    }    
    #endregion    
}
