using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityOrUpgrade : MonoBehaviour
{
    #region Debug
    [SerializeField] GameObject prefabOverride;
    [SerializeField] bool enableDebugMode;
    #endregion

    private Button activationButton;
    private Dictionary<string, AbilityUpgrades> selectedUpgrade = new Dictionary<string, AbilityUpgrades>();
    private AbilityLibraryData.AbilityStats selectedAbility = new AbilityLibraryData.AbilityStats();

    private void OnEnable() 
    {
        AbilityCardOnClick.OnCardSelection += SetAbilityUpradeProperties;
        InitializeButton();
    }

    private void OnDisable() 
    {
        AbilityCardOnClick.OnCardSelection -= SetAbilityUpradeProperties;
    }

    private void SetAbilityUpradeProperties(AbilityLibraryData.AbilityStats ability, Dictionary<string, AbilityUpgrades> upgrade)
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
