using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AbilityCardGenerator))]
public class AbilityCardAssignment : MonoBehaviour
{
    [SerializeField] GameObject[] cardPanels;
    private AbilityCardGenerator cardGenerator;
    private List<GameObject> shuffledList;
    private List<AbilityLibraryData.AbilityStats> newAbilities;
    private Dictionary<string, AbilityUpgrades> newUpgrades;

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

    private void HandleAbilityCards(List<AbilityLibraryData.AbilityStats> abilities, Dictionary<string, AbilityUpgrades> upgrades)
    {
        newAbilities = abilities;
        newUpgrades = upgrades;
        shuffledList = UtilityMethods.ShuffleList(cardPanels.ToList());

        int numOfAbilitiesToDisplay = upgrades.Count >= 2 ? 1 : (upgrades.Count == 1 ? 2 : 3);
        int finalNumOfAbilitiesToDisplay = 0;
        int finalNumOfUpgradesToDisplay = 0;
        bool willRandomlySelectAbility = false;
        bool willRandomlySelectUpgrade = false;

        if (abilities.Count == 0 && upgrades.Count == 0)
        {
            Debug.Log("All abilities and upgrades have been unlocked!");
        }
        else if (abilities.Count == 0 && upgrades.Count > 0)
        {
            finalNumOfAbilitiesToDisplay = 0;

            if (upgrades.Count > 3)
            {
                finalNumOfUpgradesToDisplay = 3;
                willRandomlySelectUpgrade = true;
            }
            else
            {
                finalNumOfUpgradesToDisplay = upgrades.Count;
            }
        }
        else if (upgrades.Count == 0 && abilities.Count > 0)
        {
            finalNumOfUpgradesToDisplay = 0;

            if (abilities.Count > 3)
            {
                finalNumOfAbilitiesToDisplay = 3;
                willRandomlySelectAbility = true;
            }
            else
            {
                finalNumOfAbilitiesToDisplay = upgrades.Count;
            }
        }
        else if (numOfAbilitiesToDisplay > abilities.Count)
        {
            finalNumOfAbilitiesToDisplay = abilities.Count;
            finalNumOfUpgradesToDisplay = 3 - abilities.Count;
        }
        else if (numOfAbilitiesToDisplay <= abilities.Count)
        {
            finalNumOfAbilitiesToDisplay = numOfAbilitiesToDisplay;
            willRandomlySelectAbility = true;
            finalNumOfUpgradesToDisplay = 3 - numOfAbilitiesToDisplay;
        }

        DisplayAbilities(finalNumOfAbilitiesToDisplay, willRandomlySelectAbility);
        DisplayUpgrades(finalNumOfUpgradesToDisplay, willRandomlySelectUpgrade);

        // for (int i = 0; i < shuffledList.Count; i++)
        // {
        //     TextMeshProUGUI[] textUIElements = shuffledList[i].GetComponentsInChildren<TextMeshProUGUI>();

        //     if (!isAbilityAssigned && abilities.Count > 0)
        //     {
        //         // j represents the number of new abilities to display based on how many upgrades are available to be displayed.
        //         if (numOfAbilitiesToDisplay > 0)
        //         {
        //             textUIElements[0].text = "New Ability!!";
        //             textUIElements[1].text = abilities[0].abilityName;
        //             textUIElements[2].text = abilities[0].abilityDescription;
        //             numOfAbilitiesToDisplay--;
        //         }
        //         else
        //         {
        //             isAbilityAssigned = true;
        //         }
        //     }
        //     else
        //     {
        //         foreach (KeyValuePair<string, AbilityUpgrades> mid in upgrades)
        //         {
        //             // mid.Key is "{abilityName}_idx{int}".  Need to remove the "_idx{int}" portion
        //             int lastIndex = mid.Key.LastIndexOf("_");
        //             string abilityName = mid.Key.Substring(0, lastIndex);
        //             string upgradeType = UtilityMethods.InsertSpaceBeforeCapitalLetters(mid.Value.upgradeType.ToString());

        //             textUIElements[0].text = "New Upgrade!!";
        //             textUIElements[1].text = $"{abilityName}\n{upgradeType}";
        //             textUIElements[2].text = mid.Value.upgradeDescription;
        //         }

        //         x++;
        //     }
        // }
    }

    private void DisplayAbilities(int finalNumOfAbilitiesToDisplay, bool willRandomlySelectAbility)
    {
        for (int i = 0; i < finalNumOfAbilitiesToDisplay; i++)
        {
            if (willRandomlySelectAbility)
            {
                int randomIndex = UtilityMethods.GenerateRandomIndex(newAbilities.Count);
                AbilityLibraryData.AbilityStats chosenAbility = newAbilities[randomIndex];
                // Add logic to compare previous chosenAbility with new chosenAbility to prevent duplication.

                TextMeshProUGUI[] textElements = shuffledList[i].GetComponentsInChildren<TextMeshProUGUI>();
                textElements[0].text = "New Ability!!";
                textElements[1].text = chosenAbility.abilityName;
                textElements[2].text = chosenAbility.abilityDescription;
            }
        }
    }

    private void DisplayUpgrades(int finalNumOfUpgradesToDisplay, bool willRandomlySelectUpgrade)
    {
        // if (finalNumOfUpgradesToDisplay > 3)
        // {
        //     finalNumOfUpgradesToDisplay = 3;
        // }

        // while (finalNumOfUpgradesToDisplay > 0)
        // {
        //     foreach (KeyValuePair<string, AbilityUpgrades> mid in newUpgrades)
        //     {
        //         // mid.Key is "{abilityName}_idx{int}".  Need to remove the "_idx{int}" portion
        //         int lastIndex = mid.Key.LastIndexOf("_");
        //         string abilityName = mid.Key.Substring(0, lastIndex);
        //         string upgradeType = UtilityMethods.InsertSpaceBeforeCapitalLetters(mid.Value.upgradeType.ToString());

        //         textUIElements[0].text = "New Upgrade!!";
        //         textUIElements[1].text = $"{abilityName}\n{upgradeType}";
        //         textUIElements[2].text = mid.Value.upgradeDescription;
        //     }

        //     numToDisplay--;
        // }
    }
}
