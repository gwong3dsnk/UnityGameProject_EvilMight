using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] Canvas playerDeathCanvas;
    public static GameManager Instance;
    private GameStates currentGameState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (playerDeathCanvas != null)
        {
            playerDeathCanvas.transform.gameObject.SetActive(false);
        }
        else
        {
            Logger.LogError("Missing PlayerDeathCanvas reference", this);
        }

        ChangeGameState(GameStates.Playing);
    }

    public void ChangeGameState(GameStates gameStates)
    {
        currentGameState = gameStates;

        switch (currentGameState)
        {
            case GameStates.MainMenu:
                TriggerMainMenu();
                break;
            case GameStates.Playing:
                TriggerPlaying();
                break;
            case GameStates.PlayerVictory:
                TriggerPlayerVictory();
                break;
            case GameStates.PlayerLoss:
                TriggerPlayerLoss();
                break;
            case GameStates.Pausing:
                TriggerPausing();
                break;
            default:
                return;
        }
    }

    private void TriggerMainMenu()
    {
        // Return to main menu.
    }

    private void TriggerPlaying()
    {
        ResumeGame();
    }

    private void TriggerPlayerVictory()
    {
        Logger.Log("Player is victorious.  Show Victory screen.", this); 
        FreezeGame();
    }

    private void TriggerPlayerLoss()
    {
        playerDeathCanvas.transform.gameObject.SetActive(true);
        FreezeGame();
    }

    private void TriggerPausing()
    {
        FreezeGame();
    }

    private void FreezeGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
