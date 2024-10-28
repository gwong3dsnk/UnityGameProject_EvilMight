using UnityEngine.SceneManagement;
using UnityEngine;

public class GlobalUI : MonoBehaviour
{
    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        GameManager.Instance.ChangeGameState(GameStates.Playing);
    }

    public void ReturnToMainMenu()
    {
        Logger.Log("Returning to Main Menu Screen");
    }
}
