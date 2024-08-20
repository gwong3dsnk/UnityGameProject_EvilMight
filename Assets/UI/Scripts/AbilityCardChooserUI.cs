using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class AbilityCardChooserUI : MonoBehaviour
{
    [SerializeField] PlayerAbilitiesManager abilityManager;
    [SerializeField] Canvas abilityChoiceCanvas;
    [SerializeField] GameObject screenAOE; // temp

    public void OnCardButtonClick(Button clickedButton)
    {
        string buttonName = clickedButton.gameObject.name;

        switch (buttonName)
        {
            case "Ability1_CardImage":
                HandleAbility1Card();
                break;
            case "Ability2_CardImage":
                HandleAbility2Card();
                break;
            case "Ability3_CardImage":
                HandleAbility3Card();
                break;
        }

        DisableAbilityChoiceCanvas();
    }

    private void HandleAbility1Card()
    {
        // This method will be a new ability (screen clear of enemies)
        // Will eventually need an if statement to see if it's a new ability or an ability upgrade.
        Debug.Log("Instantiating ScreenAOE ability gameobject", this);
        abilityManager.InstantiateAbility(screenAOE);
    }

    private void HandleAbility2Card()
    {
        // This method will be an upgrade to the SingleShotProjectile firerate
        Debug.Log("Logic for Ability 2");
    }

    private void HandleAbility3Card()
    {
        // This method will be an upgrade to the SingleShotProjectile conal spread       
        Debug.Log("Logic for Ability 3");
    }

    public void EnableAbilityChoiceCanvas()
    {
        Time.timeScale = 0;
        abilityChoiceCanvas.gameObject.SetActive(true);
    }

    private void DisableAbilityChoiceCanvas()
    {
        Time.timeScale = 1;
        abilityChoiceCanvas.gameObject.SetActive(false);
    }
}
