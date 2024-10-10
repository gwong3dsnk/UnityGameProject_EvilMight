using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesManager : MonoBehaviour
{
    [SerializeField] AbilityDatabaseManager abilityDatabaseManager;
    [SerializeField] ActivateButtonOnClick activateButtonOnClick;
    [SerializeField] GameObject playerDefaultAbility;
    private PlayerAnimController baseHandsAnimController;
    private PlayerAnimController smallHandsAnimController;
    private AbilityBase abilityToAnimate;    
    private List<AbilityBase> activeAbilities = new List<AbilityBase>();
    public List<AbilityBase> ActiveAbilities => activeAbilities;
    public static AbilitiesManager AbilityManagerInstance { get; private set; }
    public event EventHandler OnAbilityActivationCompletion;
    public event Action<AbilityBase> HandleAbilityPlayAnim;

    private void Awake()
    {
        Logger.Log($"[AbilitiesManager] - Initializing PLAYER ABILITY MANAGER singleton instance OnAwake.", this);
        baseHandsAnimController = GetComponent<AbilityHelper>().AbilityHelperData[0].meshSockets[0].renderMesh.GetComponent<PlayerAnimController>();
        smallHandsAnimController = GetComponent<AbilityHelper>().AbilityHelperData[1].meshSockets[0].renderMesh.GetComponent<PlayerAnimController>();

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
            Logger.LogError("[AbilitiesManager] - Missing references to activateButtonOnClick or AbilityDatabaseManager.", this);
        }

        if (baseHandsAnimController == null || smallHandsAnimController == null)
        {
            Logger.LogError("[AbilitiesManager] - Missing either AbilityHelper script component or AbilityHelperData on PlayerAbilityContainer gameobject", this);
        }
    }

    private void Start()
    {
        // Call this to instantiate the player's default ability right away.
        BeginUnlockingNewAbility(this, EventArgs.Empty);        
    }

    private void OnEnable() 
    {
        activateButtonOnClick.OnAbilityChosen += BeginUnlockingNewAbility;
        baseHandsAnimController.OnAnimFXPlay += PlayAnimEventFX;
        smallHandsAnimController.OnAnimFXPlay += PlayAnimEventFX;
    }

    private void OnDisable()
    {
        activateButtonOnClick.OnAbilityChosen -= BeginUnlockingNewAbility;
        baseHandsAnimController.OnAnimFXPlay -= PlayAnimEventFX;
        smallHandsAnimController.OnAnimFXPlay -= PlayAnimEventFX;        
    }

    private void PlayAnimEventFX(object sender, System.EventArgs e)
    {
        Logger.Log("[AbilitiesManager] - Working on playing anim event FX particle system.", this);
        abilityToAnimate.HandlePlayAnimEventFX();
    }    

    public void AddAbility(AbilityBase ability)
    {
        Logger.Log($"[AbilitiesManager] - Adding ability [{ability.AbilityName}] to activeAbilities.", this);
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
            Logger.Log("[AbilitiesManager] - Cast Succeeded. Instantiating from CardPanel choice.", this);
            abilityToInstantiate = activateButtonOnClick.SelectedAbilityPrefab;
            isPlayerDefaultAbility = false;
        }
        else
        {
            Logger.Log("[AbilitiesManager] - Cast Failed, one-time onAwake instantiation of playerDefaultAbility", this);
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
        Logger.Log("[AbilitiesManager] - Invoking OnActivationCompletion.", this);
        OnAbilityActivationCompletion?.Invoke(this, EventArgs.Empty); 
    }

    public void InvokeHandleAbilityPlayAnimEvent(AbilityBase ability)
    {
        abilityToAnimate = ability;
        Logger.Log("[AbilitiesManager] - Invoking HandleAbilityPlayAnim.", this);
        HandleAbilityPlayAnim?.Invoke(ability);
    }
}
