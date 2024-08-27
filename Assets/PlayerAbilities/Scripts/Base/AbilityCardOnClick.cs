using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityCardOnClick : MonoBehaviour, IPointerClickHandler
{
    // [SerializeField] PlayerAbilitiesManager abilityManager;
    [SerializeField] AbilityCardAssignment cardAssigner;
    private Dictionary<string, AbilityUpgrades> selectedUpgrade = new Dictionary<string, AbilityUpgrades>();
    private AbilityLibraryData.AbilityStats selectedAbility = new AbilityLibraryData.AbilityStats();
    public event Action<AbilityLibraryData.AbilityStats, Dictionary<string, AbilityUpgrades>> OnCardSelection;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPress != null)
        {
            // Highlight card
            // Find matching data from Assignment public properties
            IdentifyClickedCardData();
            Debug.Log("Clicked Object: " + eventData.pointerPress.name);
            OnCardSelection?.Invoke(selectedAbility, selectedUpgrade);
        }
        else
        {
            Debug.LogWarning("PointerPress is null.");
        }
    }

    private void IdentifyClickedCardData()
    {
        throw new NotImplementedException();
    }
}
