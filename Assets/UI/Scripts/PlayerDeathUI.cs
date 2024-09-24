using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathUI : MonoBehaviour
{
    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        Logger.Log("Returning to Main Menu Screen");
    }
}
