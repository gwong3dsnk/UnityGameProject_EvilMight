using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] Canvas playerDeathCanvas;

    void Start()
    {
        if (playerDeathCanvas != null)
        {
            playerDeathCanvas.transform.gameObject.SetActive(false);
        }
        else
        {
            Logger.LogError("[PlayerDeathHandler] -  Missing PlayerDeathCanvas reference", this);
        }
    }

    public void HandleDeath()
    {
        Destroy(gameObject);

        if (playerDeathCanvas != null)
        {
            Logger.Log("[PlayerDeathHandler] - Enabling Player Death Canvas");
            playerDeathCanvas.transform.gameObject.SetActive(true);
        }
        else
        {
            Logger.LogError("[PlayerDeathHandler] -  Missing PlayerDeathCanvas reference", this);
        }        
        
        Logger.Log("[PlayerDeathHandler] - Freezing time on player death.", this);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
