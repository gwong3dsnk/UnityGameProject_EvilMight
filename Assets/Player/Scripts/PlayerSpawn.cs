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
        InstantiateDefaultAbility();
    }

    private void InstantiateDefaultAbility()
    {
        PlayerAbilitiesManager.AbilityManagerInstance.InstantiateAbility(defaultStartingAbility);
    }
}
