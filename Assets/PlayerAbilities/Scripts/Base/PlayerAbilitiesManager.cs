using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAbilitiesManager : MonoBehaviour
{
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
        ability.UpgradeAbility();
    }
}
