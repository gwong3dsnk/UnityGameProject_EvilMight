using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
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
        Logger.Log("PLAYER DEAD - PLAYING DEATH COROUTINE.", this);
        AbilitiesManager.AbilityManagerInstance.DeactivatePlayerAbilities();
        playerAnimController.ProcessDeathAnim();
        yield return new WaitForSeconds(deathDelay);
        Logger.Log("PLAYER DEAD - GAMESTATE CHANGE.", this);
        GameManager.Instance.ChangeGameState(GameStates.PlayerLoss); // Player has died.  Game over. Change the game state
    }
}
