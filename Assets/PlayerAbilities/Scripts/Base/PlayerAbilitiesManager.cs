using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerAbilitiesManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] bool enableDebug;
    // private Vector3 spawnPositionOffset = new Vector3(0f, 1.7f, 2.5f);
    private PlayerAbilities currentPlayerAbility;
    public Dictionary<AbilityNames, Dictionary<UpgradeTypes, Queue<UpgradeLevelData>>> upgradeTypeDatabase;
    private List<PlayerAbilities> activeAbilities = new List<PlayerAbilities>();
    public List<PlayerAbilities> ActiveAbilities => activeAbilities;
    private Dictionary<string, AbilityUpgrades> activeUpgrades = new Dictionary<string, AbilityUpgrades>();
    public Dictionary<string, AbilityUpgrades> ActiveUpgrades => activeUpgrades;
    public static PlayerAbilitiesManager AbilityManagerInstance { get; private set; }
    public event EventHandler OnActivationCompletion;

    private void Awake()
    {
        // Initialize singleton instance
        if (AbilityManagerInstance == null)
        {
            AbilityManagerInstance = this;
        }
        else if (AbilityManagerInstance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddAbility(PlayerAbilities ability)
    {
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            ability.ActivateAbility();
        }
    }

    public void RemoveAbility(PlayerAbilities ability)
    {
        if (activeAbilities.Contains(ability))
        {
            activeAbilities.Remove(ability);
            ability.DeactivateAbility();
        }
    }

    public void AddAbilityUpgrade(Dictionary<string, AbilityUpgrades> upgrade)
    {
        KeyValuePair<string, AbilityUpgrades> upgradeKVP = upgrade.First();

        if (!activeUpgrades.ContainsKey(upgradeKVP.Key))
        {
            activeUpgrades.Add(upgradeKVP.Key, upgradeKVP.Value);
            currentPlayerAbility.ActivateUpgrade(upgrade);
        }

        InvokeOnActivationCompletion();
    }

    public void InstantiateAbility(GameObject ability)
    {
        Vector3 particleSpawnPosition = player.transform.position;
        GameObject abilityGameObject = Instantiate(ability, particleSpawnPosition, Quaternion.identity, transform);
        currentPlayerAbility = abilityGameObject.GetComponent<PlayerAbilities>();
        AddAbility(currentPlayerAbility);

        InvokeOnActivationCompletion();
    }

    private void InvokeOnActivationCompletion()
    {
        OnActivationCompletion?.Invoke(this, EventArgs.Empty);
    }
}
