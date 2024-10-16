using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] Canvas playerDeathCanvas;
    private PlayerHealth playerHealth;
    private PlayerAnimController playerAnimController;
    private float deathDelay = 1.0f;   

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Logger.LogError("[PlayerDeathHandler] -  Missing PlayerHealth reference", this);
        }
    } 

    private void Start()
    {
        playerAnimController = GetComponent<PlayerAnimController>();

        if (playerAnimController == null)
        {
            Logger.LogError($"[{this.name}] -  Missing PlayerAnimController reference", this);
        }

        if (playerDeathCanvas != null)
        {
            playerDeathCanvas.transform.gameObject.SetActive(false);
        }
        else
        {
            Logger.LogError("[PlayerDeathHandler] -  Missing PlayerDeathCanvas reference", this);
        }
    }

    private void OnEnable()
    {
        playerHealth.OnDeath += StartDeathCoroutine;
    }

    private void OnDisable()
    {
        playerHealth.OnDeath -= StartDeathCoroutine;
    }

    private void StartDeathCoroutine(object sender, System.EventArgs e)
    {
        if (sender as PlayerHealth)
        {
            StartCoroutine(PlayerDeathCoroutine());
        }
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        playerAnimController.ProcessDeathAnim();
        yield return new WaitForSeconds(deathDelay);
        DeathSequence();
    }

    private void DeathSequence()
    {
        playerDeathCanvas.transform.gameObject.SetActive(true);

        Logger.Log("[PlayerDeathHandler] - Freezing time on player death.", this);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
