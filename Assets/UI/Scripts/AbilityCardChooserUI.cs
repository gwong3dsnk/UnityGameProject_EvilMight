using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardChooserUI : MonoBehaviour
{
    [SerializeField] PlayerAbilitiesManager abilityManager;
    [SerializeField] Canvas abilityChoiceCanvas;
    [SerializeField] LevelManager levelManager;

    private void Start()
    {
        if (levelManager == null)
        {
            Debug.LogError("Missing level manager reference", this);
        }
        // else
        // {
        //     levelManager.OnLevelUp += EnableAbilityChoiceCanvas;
        // }
    }

    private void OnEnable() 
    {
        levelManager.OnLevelUp += EnableAbilityChoiceCanvas;
    }

    private void OnDisable() 
    {
        levelManager.OnLevelUp -= EnableAbilityChoiceCanvas;
    }

    public void ProcessSelectedCard()
    {
        // TODO : Get the ability/upgrade data from the card the player has selected.
        // If new ability, AddAbility() and InstantiateAbility() through PlayerAbilitiesManager
        // If ability upgrade, AddAbilityUpgrade() to activeUpgrades and apply the upgrade data to the respective ability particle system

        DisableAbilityChoiceCanvas();
    }

    private void HandleAbility1Card()
    {
        // This method is currently handling JUST ScreenAOE.  Should eventually be updated.
        // Will eventually need an if statement to see if it's a new ability or an ability upgrade.
        Debug.Log("Instantiating ScreenAOE ability gameobject", this);
        PlayerAbilities screenAOE = FindObjectOfType<ScreenAOE>();
        GameObject screenAOEObject = screenAOE.transform.gameObject;
        abilityManager.InstantiateAbility(screenAOEObject);
    }

    private void HandleAbility2Card()
    {
        // This method will be an upgrade to the Single_Shot firerate by increase Emission rate from 1 to 2.
        // Check if the player has the ability and that it's active
        Debug.Log("Upgrading SingleShot Firerate (Emission)");
        PlayerAbilities singleShot = FindObjectOfType<Single_Shot>();
        singleShot.UpgradeAbility();
    }

    private void HandleAbility3Card()
    {
        // This method will be an upgrade to the SingleShotProjectile conal spread       
        Debug.Log("Logic for Ability 3");
    }

    public void EnableAbilityChoiceCanvas(object sender, System.EventArgs e)
    {
        Time.timeScale = 0;
        abilityChoiceCanvas.gameObject.SetActive(true);

        AbilityCardGenerator cardGenerator = abilityChoiceCanvas.GetComponent<AbilityCardGenerator>();
        if (cardGenerator != null)
        {
            cardGenerator.GenerateAbilityCards();
        }
        else
        {
            Debug.LogError("Missing reference to AbilityCardGenerator", this);
        }
    }

    private void DisableAbilityChoiceCanvas()
    {
        Time.timeScale = 1;
        abilityChoiceCanvas.gameObject.SetActive(false);
    }
}
