using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject defaultStartingAbility;
    private PlayerAbilitiesManager abilityManager;

    void Start()
    {
        abilityManager = GetComponentInChildren<PlayerAbilitiesManager>();

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        InstantiateDefaultAbility();
    }

    private void InstantiateDefaultAbility()
    {
        abilityManager.InstantiateAbility(defaultStartingAbility);
    }
}
