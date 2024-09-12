using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class ActivateButtonOnClick : MonoBehaviour
{
    #region Debug
    [SerializeField] GameObject prefabOverride;
    [SerializeField] bool enableDebugMode;
    #endregion

    private Button activationButton;
    private UpgradeTypesDatabase selectedUpgrade = new UpgradeTypesDatabase();
    private AbilityLibraryData.AbilityStats selectedAbility = new AbilityLibraryData.AbilityStats();
    public static event Action<GameObject> OnAbilityChosen;
    public static event Action<UpgradeTypesDatabase> OnUpgradeChosen;

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
        if (selectedAbility != null)
        {
            Logger.Log("Ability Chosen");
            GameObject abilityPrefab = DebugAbilityOverride(selectedAbility);
            OnAbilityChosen?.Invoke(abilityPrefab);
        }
        else if (selectedUpgrade.Count > 0)
        {
            Logger.Log("Upgrade Chosen");
            OnUpgradeChosen?.Invoke(selectedUpgrade);
        }
    }

    #region Debug
    private GameObject DebugAbilityOverride(AbilityLibraryData.AbilityStats selectedAbility)
    {
        GameObject abilityPrefab = null;

        if (enableDebugMode)
        {
            abilityPrefab = prefabOverride;
        }
        else
        {
            abilityPrefab = selectedAbility.prefab;
        }    

        return abilityPrefab;    
    }    
    #endregion
}
