using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject defaultStartingAbility;

    void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        InstantiateDefaultAbility();
    }

    private void InstantiateDefaultAbility()
    {
        PlayerAbilitiesManager.AbilityManagerInstance.InstantiateAbility(defaultStartingAbility);
    }
}
