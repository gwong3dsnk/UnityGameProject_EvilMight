using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private Canvas playerDeathCanvas;
    [SerializeField] private Canvas pauseCanvas;
    public static GameManager Instance;
    private GameStates currentGameState;
    private PlayerControls playerControls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (playerDeathCanvas == null || pauseCanvas == null)
        {
            Logger.LogError("Missing PlayerDeathCanvas or PauseCanvas reference", this);
        }

        playerControls = new PlayerControls();

        ChangeGameState(GameStates.Playing);
    }

    private void OnEnable() 
    {
        playerControls.Enable();
        playerControls.Player.Pause.performed += OnPause;
    }    

    private void OnDisable() 
    {
        playerControls.Disable();
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
            case GameStates.LevelingUp:
                TriggerLevelingUp();
                break;
            default:
                return;
        }
    }

    public void SetGameStateWhenResuming()
    {
        // Called when clicking Resume game button in GUI
        ChangeGameState(GameStates.Playing);
        pauseCanvas.enabled = false;
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
        playerDeathCanvas.enabled = true;
        FreezeGame();
    }

    private void TriggerPausing()
    {
        pauseCanvas.enabled = true;
        FreezeGame();
    }

    private void TriggerLevelingUp()
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

    private void OnPause(InputAction.CallbackContext value)
    {
        ChangeGameState(GameStates.Pausing);
    }
}
