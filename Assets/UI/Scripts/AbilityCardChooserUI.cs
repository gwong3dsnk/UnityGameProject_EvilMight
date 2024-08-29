using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardChooserUI : MonoBehaviour
{
    [SerializeField] Canvas abilityChoiceCanvas;
    [SerializeField] LevelManager levelManager;

    private void Start()
    {
        if (levelManager == null)
        {
            Debug.LogError("Missing level manager reference", this);
        }
    }

    private void OnEnable() 
    {
        levelManager.OnLevelUp += EnableAbilityChoiceCanvas;
        PlayerAbilitiesManager.AbilityManagerInstance.OnActivationCompletion += DisableAbilityChoiceCanvas;
    }

    private void OnDisable() 
    {
        levelManager.OnLevelUp -= EnableAbilityChoiceCanvas;
        PlayerAbilitiesManager.AbilityManagerInstance.OnActivationCompletion -= DisableAbilityChoiceCanvas;
    }

    public void EnableAbilityChoiceCanvas(object sender, System.EventArgs e)
    {
        Time.timeScale = 0;
        abilityChoiceCanvas.gameObject.SetActive(true);

        AbilityCardGenerator cardGenerator = abilityChoiceCanvas.GetComponent<AbilityCardGenerator>();
        if (cardGenerator != null)
        {
            cardGenerator.BeginGeneration();
        }
        else
        {
            Debug.LogError("Missing reference to AbilityCardGenerator", this);
        }
    }

    public void DisableAbilityChoiceCanvas(object sender, System.EventArgs e)
    {
        Time.timeScale = 1;
        abilityChoiceCanvas.gameObject.SetActive(false);
    }
}
