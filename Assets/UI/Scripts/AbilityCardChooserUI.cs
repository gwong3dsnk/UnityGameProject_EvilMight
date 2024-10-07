using UnityEngine;

public class AbilityCardChooserUI : MonoBehaviour
{
    [SerializeField] Canvas abilityChoiceCanvas;
    [SerializeField] LevelManager levelManager;

    private void Start()
    {
        Logger.Log("[AbilityCardChooseUI] - ABILITY CARD CHOOSE UI OnStart", this);
        Logger.Log("------------------------------------------------", this);
        Logger.Log("[AbilityCardChooseUI] - All OnAwake logic and AbilityCardChoose OnStart logic FINISHED", this);
        Logger.Log("------------------------------------------------", this);
        if (levelManager == null)
        {
            Logger.LogError("[AbilityCardChooseUI] - Missing level manager reference", this);
        }
    }

    private void OnEnable() 
    {
        levelManager.OnLevelUp += EnableAbilityChoiceCanvas;
        AbilitiesManager.AbilityManagerInstance.OnAbilityActivationCompletion += DisableAbilityChoiceCanvas;
        UpgradeManager.UpgradeManagerInstance.OnUpgradeActivationCompletion += DisableAbilityChoiceCanvas;
    }

    private void OnDisable() 
    {
        levelManager.OnLevelUp -= EnableAbilityChoiceCanvas;
        AbilitiesManager.AbilityManagerInstance.OnAbilityActivationCompletion -= DisableAbilityChoiceCanvas;
        UpgradeManager.UpgradeManagerInstance.OnUpgradeActivationCompletion -= DisableAbilityChoiceCanvas;
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
            Logger.LogError("[AbilityCardChooseUI] - Missing reference to AbilityCardGenerator", this);
        }
    }

    public void DisableAbilityChoiceCanvas(object sender, System.EventArgs e)
    {
        Logger.Log("[AbilityCardChooseUI] - Resuming time and disabling ability choice canvas.", this);
        Time.timeScale = 1;
        abilityChoiceCanvas.gameObject.SetActive(false);
    }
}
