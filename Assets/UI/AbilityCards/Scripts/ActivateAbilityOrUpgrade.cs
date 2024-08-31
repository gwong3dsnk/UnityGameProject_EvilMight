using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActivateAbilityOrUpgrade : MonoBehaviour
{
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
            GameObject playerPrefab = selectedAbility.prefab;
            PlayerAbilitiesManager.AbilityManagerInstance.InstantiateAbility(playerPrefab);
        }
        else if (selectedUpgrade.Count > 0)
        {
            // First add the new upgrade to ActiveUpgrades in PlayerAbilitiesManager
            // Next process the upgrade by passing the new FX value into the respective PlayerAbilities UpgradeAbility()
            // MAY have to grab reference to the prefab in the scene instead of using passed in PlayerAbilities from
            // the AbilityLibraryData
            PlayerAbilitiesManager.AbilityManagerInstance.AddAbilityUpgrade(selectedUpgrade);
        }
    }
}
