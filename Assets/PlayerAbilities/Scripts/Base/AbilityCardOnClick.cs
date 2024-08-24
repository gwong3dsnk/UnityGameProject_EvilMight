using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityCardOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PlayerAbilitiesManager abilityManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(transform.name);
        if (abilityManager != null)
        {
            // abilityManager.AddAbilityUpgrade();
        }
    }
}
