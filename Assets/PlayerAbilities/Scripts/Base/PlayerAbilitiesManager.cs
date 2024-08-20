using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAbilitiesManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector3 spawnPositionOffset = new Vector3(0f, 1.7f, 2.5f);
    private List<PlayerAbilities> activeAbilities = new List<PlayerAbilities>();

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

    public void UpgradeAbility(PlayerAbilities ability)
    {
        // ability.UpgradeAbility();
    }

    public void InstantiateAbility(GameObject ability)
    {
        Vector3 particleSpawnPosition = player.transform.position + spawnPositionOffset;
        GameObject abilityGameObject = Instantiate(ability, particleSpawnPosition, Quaternion.identity, transform);
        PlayerAbilities abilityScript = abilityGameObject.GetComponent<PlayerAbilities>();
        AddAbility(abilityScript);
    }
}
