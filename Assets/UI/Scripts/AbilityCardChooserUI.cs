using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AbilityCardChooserUI : MonoBehaviour
{
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
    }

    private void HandleAbility1Card()
    {
        // This method will be a new ability (screen clear of enemies)
        Debug.Log("Logic for Ability 1");
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
}
