using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class ActivateAbilityOrUpgrade : MonoBehaviour
{
    #region Debug
    [SerializeField] GameObject prefabOverride;
    [SerializeField] bool enableDebugMode;
    #endregion

    private Button activationButton;
    private UpgradeTypesDatabase selectedUpgrade = new UpgradeTypesDatabase();
    private AbilityLibraryData.AbilityStats selectedAbility = new AbilityLibraryData.AbilityStats();

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
            GameObject abilityPrefab = DebugAbilityOverride(selectedAbility);
            PlayerAbilitiesManager.AbilityManagerInstance.InstantiateAbility(abilityPrefab);
        }
        else if (selectedUpgrade.Count > 0)
        {
            PlayerAbilitiesManager.AbilityManagerInstance.AddAbilityUpgrade(selectedUpgrade);
            PlayerAbilitiesManager.AbilityManagerInstance.BeginUpgradeActivation(selectedUpgrade);
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
