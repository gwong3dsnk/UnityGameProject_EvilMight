using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

[RequireComponent(typeof(Outline))]
public class AbilityCardOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] DisplayAbilityCards displayAbilityCards;
    [SerializeField] DisplayUpgradeCards displayUpgradeCards;
    private static AbilityCardOnClick currentSelectedCard;
    private UpgradeTypesDatabase selectedUpgrade = null;
    public UpgradeTypesDatabase SelectedUpgrade => selectedUpgrade;
    private AbilityLibraryData.AbilityStats selectedAbility = null;
    public AbilityLibraryData.AbilityStats SelectedAbility => selectedAbility;
    public static event Action<AbilityLibraryData.AbilityStats, UpgradeTypesDatabase> OnCardSelection;

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedAbility = null;
        selectedUpgrade = null;

        if (eventData.pointerPress != null)
        {
            ToggleCardOutline();

            GameObject selectedCardPanel = eventData.pointerPress.transform.parent.gameObject;
            IdentifyClickedCardData(selectedCardPanel);
            OnCardSelection?.Invoke(selectedAbility, selectedUpgrade);
        }
        else
        {
            Logger.LogWarning("PointerPress is null.");
        }
    }

    private void ToggleCardOutline()
    {
        DisableCardOutline();
        currentSelectedCard = this;
        EnableCardOutline();
    }

    private void IdentifyClickedCardData(GameObject selectedCardPanel)
    {
        if (displayUpgradeCards.UpgradeCardRelationship.ContainsKey(selectedCardPanel))
        {
            selectedUpgrade = displayUpgradeCards.UpgradeCardRelationship[selectedCardPanel];
        }
        else if (displayAbilityCards.AbilityCardRelationship.ContainsKey(selectedCardPanel))
        {
            selectedAbility = displayAbilityCards.AbilityCardRelationship[selectedCardPanel];
        }
        else
        {
            Logger.LogError("No matching Panel Key found!", this);
        }
    }    

    private void DisableCardOutline()
    {
        if (currentSelectedCard != null && currentSelectedCard != this)
        {
            Outline cardOutline = currentSelectedCard.GetComponent<Outline>();
            ToggleOutline(cardOutline, false);
        }
    }

    private void EnableCardOutline()
    {
        Outline outline = GetComponent<Outline>();
        ToggleOutline(outline, true);
    }

    private void ToggleOutline(Outline outline, bool state)
    {
        if (outline == null)
        {
            Logger.LogError("No Outline component found", this);
        }
        else
        {
            outline.enabled = state;
        }
    }
}
