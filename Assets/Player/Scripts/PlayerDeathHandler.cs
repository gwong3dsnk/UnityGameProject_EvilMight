using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
            Logger.LogError("PlayerDeathHandle is missing PlayerDeathCanvas reference", this);
        }
    }

    public void HandleDeath()
    {
        Destroy(gameObject);

        if (playerDeathCanvas != null)
        {
            Logger.Log("Enabling Player Death Canvas");
            playerDeathCanvas.transform.gameObject.SetActive(true);
        }
        else
        {
            Logger.LogError("PlayerDeathHandle is missing PlayerDeathCanvas reference", this);
        }        
        
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
