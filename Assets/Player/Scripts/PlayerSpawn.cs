using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject defaultStartingAbility;

    void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        Logger.Log("Initializing PLAYER SPAWN and default player ability", this);
        if (defaultStartingAbility != null)
        {
            InstantiateDefaultAbility();
        }
        else
        {
            Logger.Log("No default ability prefab has been assigned to PlayerSpawn script.", this);
        }
    }

    private void InstantiateDefaultAbility()
    {
        PlayerAbilitiesManager.AbilityManagerInstance.BeginUnlockingNewAbility(defaultStartingAbility);
    }
}
