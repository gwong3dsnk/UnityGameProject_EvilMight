using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using AbilityDataDictionary = System.Collections.Generic.Dictionary<AbilityUpgradeType, AbilityLibraryData.AbilityStats>;

[RequireComponent(typeof(AbilityCardGenerator))]
public class AbilityCardAssignment : MonoBehaviour
{
    // This class will take in the chosen ability, grab the data, and update the card text UI objects
    [SerializeField] GameObject[] cardPanels;
    private AbilityCardGenerator cardGenerator;

    private void Awake()
    {
        cardGenerator = GetComponent<AbilityCardGenerator>();
        if (cardGenerator == null)
        {
            Debug.LogError("Missing reference to AbilityCardGenerator component", this);
        }
    }

    private void OnEnable() 
    {
        cardGenerator.OnAbilitiesGenerated += HandleAbilityCards;
    }

    private void OnDisable()
    {
        cardGenerator.OnAbilitiesGenerated -= HandleAbilityCards;
    }

    private void HandleAbilityCards(AbilityLibraryData.AbilityStats ability, List<Dictionary<string, AbilityDataDictionary>> upgrades)
    {
        IList<GameObject> shuffledList = UtilityMethods.ShuffleList(cardPanels);
        for (int i = 0; i < shuffledList.Count; i++)
        {
            TextMeshProUGUI[] textUIElements = shuffledList[i].GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in textUIElements)
            {
                // FIrst is title, second is description, always.  
                // Get this data from ability and apply to first index, then increment, then do the upgrades, then increment again to break
                // out of the for loop
                Debug.Log(text.name);
            }
        }
    }
}
