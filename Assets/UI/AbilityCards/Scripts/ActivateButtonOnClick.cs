using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class ActivateButtonOnClick : MonoBehaviour
{
    #region Fields and Properties
    // Public Properties / Events
    // public AbilityNames SelectedAbilityName => selectedAbilityName;
    public AbilityLibraryData.AbilityStats SelectedAbility => selectedAbility;
    public event EventHandler OnAbilityChosen;
    public event Action<UpgradeTypesDatabase> OnUpgradeChosen;

    // Private Fields
    private Button activationButton;
    private UpgradeTypesDatabase selectedUpgrade = new UpgradeTypesDatabase();
    private AbilityLibraryData.AbilityStats selectedAbility = new AbilityLibraryData.AbilityStats();
    // private AbilityNames selectedAbilityName;
    #endregion

    private void Start()
    {
        InitializeButton();
    }

    private void OnEnable() 
    {
        AbilityCardOnClick.OnCardSelection += SetAbilityUpgradeProperties;
    }

    private void OnDisable() 
    {
        AbilityCardOnClick.OnCardSelection -= SetAbilityUpgradeProperties;
    }

    private void SetAbilityUpgradeProperties(AbilityLibraryData.AbilityStats ability, UpgradeTypesDatabase upgrade)
    {
        selectedAbility = ability;
        selectedUpgrade = upgrade;
    }

    private void InitializeButton()
    {
        activationButton = GetComponent<Button>();
        if (activationButton != null)
        {
            activationButton.onClick.AddListener(HandleButtonClicked);
        }
    }

    private void HandleButtonClicked()
    {
        Logger.Log("------------------------------------------------", this);
        Logger.Log("USER ONCLICK REGISTERED ON ACTIVATE BUTTON", this);
        if (selectedAbility != null)
        {
            Logger.Log($"Ability Chosen - {selectedAbility.abilityName}");
            OnAbilityChosen?.Invoke(this, EventArgs.Empty);
        }
        else if (selectedUpgrade.Count > 0)
        {
            Logger.Log($"Upgrade Chosen - {selectedUpgrade.First().Value.First().Key}");
            OnUpgradeChosen?.Invoke(selectedUpgrade);
        }
    }
}
