using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyWaveController))]
public class EnemyPool : MonoBehaviour
{
    #region Fields and Properties
    // SerializedFields
    [SerializeField] private EnemyData enemyData;

    // Public Fields/Properties/Events
    public Dictionary<string, EnemyWaveData[]> WaveDataSets => waveDataSets;
    public Dictionary<string, Queue<GameObject>> WaveEnemies => waveEnemies;

    // Private Fields/Properties
    private Dictionary<string, EnemyWaveData[]> waveDataSets = new();
    private Dictionary<string, Queue<GameObject>> waveEnemies = new();
    private GameObject waveContainer;
    private EnemyData.EnemyStats[] enemyStats;
    private EnemyFactory enemyFactory;
    private int totalEnemyCounter;
    #endregion

    private void Awake()
    {
        enemyFactory = GetComponent<EnemyFactory>();

        if (enemyData == null || enemyFactory == null)
        {
            Logger.LogError($"{this.name} - Missing reference to either EnemyData or EnemyFactory.", this);
            return;
        }

        CreateWaveDataRelationship();
        PopulateEnemyPool();
    }

    public bool DecrementFinalWaveCounter()
    {
        totalEnemyCounter--;

        if (totalEnemyCounter <= 0)
        {
            Logger.Log("LAST ENEMY DEAD - GAMESTATE CHANGE", this);
            StartCoroutine(ChangeGameState());
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator ChangeGameState()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.ChangeGameState(GameStates.PlayerVictory); // All enemies dead.  Game over.  Change game state.
    }

    private void CreateWaveDataRelationship()
    {
        EnemyWaveController enemyWaveController = GetComponent<EnemyWaveController>();

        if (enemyWaveController != null)
        {
            EnemyWaveContainer[] waveContainers = enemyWaveController.EnemyWaveContainer;
            int waveNumber = 1;

            foreach (EnemyWaveContainer container in waveContainers)
            {
                waveDataSets.Add($"Wave{waveNumber}", container.enemyWaveData);
                waveNumber++;
            }
        }
        else
        {
            Logger.LogError($"{this.name} - Reference to Enemy Wave Controller script not found.", this);
        }
    }

    private void PopulateEnemyPool()
    {
        enemyStats = enemyData.enemyStatsArray;
        enemyFactory.Initialize(enemyStats);
        
        foreach (KeyValuePair<string, EnemyWaveData[]> wave in waveDataSets)
        {
            Queue<GameObject> enemies = new Queue<GameObject>();
            waveContainer = new GameObject(wave.Key) { transform = { parent = transform } };

            foreach (EnemyWaveData data in wave.Value)
            {
                totalEnemyCounter += data.enemyCount;
                enemies = enemyFactory.CreateEnemy(data, waveContainer);  
            }

            // Add to list to later use to enable enemies during gameplay.
            waveEnemies[wave.Key] = enemies;
        }
    }
}
