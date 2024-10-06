using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class PlayerAbilitiesManager : MonoBehaviour
{
    [SerializeField] AbilityDatabaseManager abilityDatabaseManager;
    [SerializeField] UpgradeDatabaseManager upgradeDatabaseManager;
    [SerializeField] ActivateButtonOnClick activateButtonOnClick;
    [SerializeField] GameObject playerDefaultAbility;
    // [SerializeField] private GameObject[] playerMeshObjects; // Delete
    // private AbilityHelper abilityHelper; // Delete
    private PlayerAnimController baseHandsAnimController;
    private PlayerAnimController smallHandsAnimController;
    private PlayerAbilities abilityToAnimate;    
    // public GameObject[] PlayerMeshObjects => playerMeshObjects;
    private List<PlayerAbilities> activeAbilities = new List<PlayerAbilities>();
    public List<PlayerAbilities> ActiveAbilities => activeAbilities;
    private UpgradeTypesDatabase activeUpgrades = new UpgradeTypesDatabase();
    public UpgradeTypesDatabase ActiveUpgrades => activeUpgrades;
    public static PlayerAbilitiesManager AbilityManagerInstance { get; private set; }
    public event EventHandler OnAbilityActivationCompletion;
    public event EventHandler OnUpgradeActivationCompletion;
    public event Action<PlayerAbilities> HandleAbilityPlayAnim;

    private void Awake()
    {
        Logger.Log($"[PlayerAbilitiesManager] - Initializing PLAYER ABILITY MANAGER singleton instance OnAwake.", this);
        // abilityHelper = GetComponent<AbilityHelper>();
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

        if (abilityDatabaseManager == null || upgradeDatabaseManager == null)
        {
            Logger.LogError("[PlayerAbilitiesManager] - Missing references to either ability or upgrade database manager scripts.", this);
        }

        if (activateButtonOnClick == null)
        {
            Logger.LogError("[PlayerAbilitiesManager] - Missing references to activateButtonOnClick or AbilityHelper.", this);
        }

        if (baseHandsAnimController == null || smallHandsAnimController == null)
        {
            Logger.LogError("[PlayerAbilitiesManager] - Missing either AbilityHelper script component or AbilityHelperData on PlayerAbilityContainer gameobject", this);
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
        activateButtonOnClick.OnUpgradeChosen += AddAbilityUpgrade;
        baseHandsAnimController.OnAnimFXPlay += PlayAnimEventFX;
        smallHandsAnimController.OnAnimFXPlay += PlayAnimEventFX;
        // playerMeshObjects[0].GetComponent<PlayerAnimController>().OnAnimFXPlay += PlayAnimEventFX; // need to listen to sk small h ands close
    }

    private void OnDisable()
    {
        activateButtonOnClick.OnAbilityChosen -= BeginUnlockingNewAbility;
        activateButtonOnClick.OnUpgradeChosen -= AddAbilityUpgrade;
        baseHandsAnimController.OnAnimFXPlay -= PlayAnimEventFX;
        smallHandsAnimController.OnAnimFXPlay -= PlayAnimEventFX;        
        // playerMeshObjects[0].GetComponent<PlayerAnimController>().OnAnimFXPlay -= PlayAnimEventFX;
    }

    private void PlayAnimEventFX(object sender, System.EventArgs e)
    {
        Logger.Log("[PlayerAbilitiesManager] - Working on playing anim event FX particle system.", this);
        abilityToAnimate.HandlePlayAnimEventFX();

        // foreach (var ability in activeAbilities)
        // {
        //     if (ability.AbilityName == AbilityNames.FingerFlick)
        //     {
        //         ability.HandlePlayAnimEventFX();
        //     }
        // }
    }    

    public void AddAbility(PlayerAbilities ability)
    {
        Logger.Log($"[PlayerAbilitiesManager] - Adding ability [{ability.AbilityName}] to activeAbilities.", this);
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            ability.ActivateAbility(ability);
        }
    }

    public void RemoveAbility(PlayerAbilities ability)
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
            Logger.Log("[PlayerAbilitiesManager] - Cast Succeeded. Instantiating from CardPanel choice.", this);
            abilityToInstantiate = activateButtonOnClick.SelectedAbilityPrefab;
            isPlayerDefaultAbility = false;
        }
        else
        {
            Logger.Log("[PlayerAbilitiesManager] - Cast Failed, one-time onAwake instantiation of playerDefaultAbility", this);
            abilityToInstantiate = playerDefaultAbility;
            isPlayerDefaultAbility = true;
        }

        // Start with instantiating the ability prefab.
        GameObject abilityGameObject = Instantiate(abilityToInstantiate, transform.position, Quaternion.identity, transform);

        // Process AddAbility.
        AddAbility(abilityGameObject.GetComponent<PlayerAbilities>());

        // Remove the unlocked ability from the ability database so it won't be shown in future level-ups.
        abilityDatabaseManager.RemoveAbilityFromDatabase(abilityGameObject.GetComponent<PlayerAbilities>());

        if (!isPlayerDefaultAbility)
        {
            InvokeOnActivationCompletion();
        }
    }

    public void AddAbilityUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log("Starting AddAbilityUpgrade", this);
        Dictionary<AbilityNames, UpgradeTypes> upgradeToDequeue = new Dictionary<AbilityNames, UpgradeTypes>();
        AbilityNames newAbilityName = newUpgrade.First().Key;
        UpgradeTypes newUpgradeType = newUpgrade.First().Value.First().Key;
        Queue<UpgradeLevelData> newQueue = newUpgrade.First().Value.First().Value;

        if (activeUpgrades.ContainsKey(newAbilityName))
        {
            if (!activeUpgrades[newAbilityName].ContainsKey(newUpgradeType))
            {
                Logger.Log("Ability exists in ActiveUpgrades, upgrade type doesn't.  Adding only upgrade entry.", this); 
                activeUpgrades[newAbilityName].Add(newUpgradeType, newQueue);
            }
            else
            {
                Logger.Log("Ability exists in ActiveUpgrades, upgrade type exists.  Don't need to do anything.", this);
            }
        }
        else
        {
            Logger.Log("Ability DOES NOT exist in ActiveUpgrades.  Adding ability & upgrade entry.", this); 
            activeUpgrades.Add(newAbilityName, newUpgrade.First().Value);
        }

        Logger.Log($"Upgrade Added: [{newUpgrade.First().Key}] + [{newUpgrade.First().Value.First().Key}]");

        BeginUpgradeActivation(newUpgrade);

        // Remove the upgrade LevelData from the UpgradeDatabase so it won't be displayed again on future level-ups.
        upgradeToDequeue.Add(newAbilityName, newUpgradeType);
        upgradeDatabaseManager.ProcessDequeue(upgradeToDequeue);

        OnUpgradeActivationCompletion?.Invoke(this, EventArgs.Empty);
    }

    public void BeginUpgradeActivation(UpgradeTypesDatabase newUpgrade)
    {
        Logger.Log("------------------------------------------------", this);
        Logger.Log("Starting BeginUpgradeActivation", this);
        AbilityNames newAbilityName = newUpgrade.First().Key;

        foreach (PlayerAbilities ability in activeAbilities)
        {
            if (ability.AbilityName == newAbilityName)
            {
                Logger.Log("Ability match found in activeAbilities. Calling ActivateUpgrade", this);
                ability.ActivateUpgrade(newUpgrade);
            }
        }
    }

    public void InvokeOnActivationCompletion()
    {
        Logger.Log("Invoking OnActivationCompletion in PlayerAbilitiesManager.", this);
        OnAbilityActivationCompletion?.Invoke(this, EventArgs.Empty); 
    }

    public void InvokeHandleAbilityPlayAnimEvent(PlayerAbilities ability)
    {
        abilityToAnimate = ability;
        Logger.Log("Invoking HandleAbilityPlayAnim in PlayerAbilitiesManager.", this);
        HandleAbilityPlayAnim?.Invoke(ability);
    }
}
