using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] Canvas playerDeathCanvas;

    void Start()
    {
        if (playerDeathCanvas != null)
        {
            playerDeathCanvas.enabled = false;
        }
        else
        {
            Debug.LogError("PlayerDeathHandle is missing PlayerDeathCanvas reference", this);
        }
    }

    public void HandleDeath()
    {
        Destroy(gameObject);

        if (playerDeathCanvas != null)
        {
            playerDeathCanvas.enabled = true;
        }
        else
        {
            Debug.LogError("PlayerDeathHandle is missing PlayerDeathCanvas reference", this);
        }        
        
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
