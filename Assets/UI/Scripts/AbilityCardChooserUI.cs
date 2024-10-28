using UnityEngine;

public class AbilityCardChooserUI : MonoBehaviour
{
    [SerializeField] Canvas abilityChoiceCanvas;
    [SerializeField] PlayerLevelingManager playerLevelingManager;

    private void Start()
    {
        Logger.Log("[AbilityCardChooseUI] - ABILITY CARD CHOOSE UI OnStart", this);
        Logger.Log("------------------------------------------------", this);
        Logger.Log("[AbilityCardChooseUI] - All OnAwake logic and AbilityCardChoose OnStart logic FINISHED", this);
        Logger.Log("------------------------------------------------", this);
        if (playerLevelingManager == null)
        {
            Logger.LogError("[AbilityCardChooseUI] - Missing PlayerLevelingManager reference", this);
            return;
        }
    }

    private void OnEnable() 
    {
        playerLevelingManager.OnLevelUp += EnableAbilityChoiceCanvas;
        AbilitiesManager.AbilityManagerInstance.OnAbilityActivationCompletion += DisableAbilityChoiceCanvas;
        UpgradeManager.UpgradeManagerInstance.OnUpgradeActivationCompletion += DisableAbilityChoiceCanvas;
    }

    private void OnDisable() 
    {
        playerLevelingManager.OnLevelUp -= EnableAbilityChoiceCanvas;
        AbilitiesManager.AbilityManagerInstance.OnAbilityActivationCompletion -= DisableAbilityChoiceCanvas;
        UpgradeManager.UpgradeManagerInstance.OnUpgradeActivationCompletion -= DisableAbilityChoiceCanvas;
    }

    public void EnableAbilityChoiceCanvas(object sender, System.EventArgs e)
    {
        GameManager.Instance.ChangeGameState(GameStates.LevelingUp);        
        abilityChoiceCanvas.enabled = true;

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
        GameManager.Instance.ChangeGameState(GameStates.Playing);
        abilityChoiceCanvas.enabled = false;
    }
}
