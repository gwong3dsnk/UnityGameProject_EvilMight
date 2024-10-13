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

    // Public Fields/Properties/Events
    public static AbilitiesManager AbilityManagerInstance { get; private set; }
    public event EventHandler OnAbilityActivationCompletion;
    public event Action<AbilityBase> HandleAbilityPlayAnim;
    public List<AbilityBase> ActiveAbilities => activeAbilities;

    // Private Fields
    private PlayerAnimController baseHandsAnimController;
    private PlayerAnimController smallHandsAnimController;
    // private AbilityBase abilityToAnimate;    
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
        }

        if (baseHandsAnimController == null || smallHandsAnimController == null)
        {
            Logger.LogError($"[{this.name}] - Missing either AbilityHelper script component or AbilityHelperData on PlayerAbilityContainer gameobject", this);
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
        Logger.Log($"[{this.name}] - Adding ability [{ability.AbilityName}] to activeAbilities.", this);
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            ability.ActivateAbility();
        }
    }

    public void RemoveAbility(AbilityBase ability)
    {
        if (activeAbilities.Contains(ability))
        {
            activeAbilities.Remove(ability);
            ability.DeactivateAbility();
        }
    }

    public void BeginUnlockingNewAbility(object sender, System.EventArgs e)
    {
        ActivateButtonOnClick activateAbilityButtonClicked = sender as ActivateButtonOnClick;
        GameObject abilityToInstantiate;
        bool isPlayerDefaultAbility;

        if (activateAbilityButtonClicked != null)
        {
            Logger.Log($"[{this.name}] - Cast Succeeded. Instantiating from CardPanel choice.", this);
            abilityToInstantiate = activateButtonOnClick.SelectedAbilityPrefab;
            isPlayerDefaultAbility = false;
        }
        else
        {
            Logger.Log($"[{this.name}] - Cast Failed, one-time onAwake instantiation of playerDefaultAbility", this);
            abilityToInstantiate = playerDefaultAbility;
            isPlayerDefaultAbility = true;
        }

        // Start with instantiating the ability prefab.
        GameObject abilityGameObject = Instantiate(abilityToInstantiate, transform.position, Quaternion.identity, transform);

        // Process AddAbility.
        AddAbility(abilityGameObject.GetComponent<AbilityBase>());

        // Remove the unlocked ability from the ability database so it won't be shown in future level-ups.
        abilityDatabaseManager.RemoveAbilityFromDatabase(abilityGameObject.GetComponent<AbilityBase>());

        if (!isPlayerDefaultAbility)
        {
            InvokeOnActivationCompletion();
        }
    }

    public void InvokeOnActivationCompletion()
    {
        Logger.Log($"[{this.name}] - Invoking OnActivationCompletion.", this);
        OnAbilityActivationCompletion?.Invoke(this, EventArgs.Empty); 
    }

    public void InvokeHandleAbilityPlayAnimEvent(AbilityBase ability)
    {
        // abilityToAnimate = ability;
        // Logger.Log($"[{this.name}] - Invoking HandleAbilityPlayAnim for {abilityToAnimate.AbilityName}.", this);
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
