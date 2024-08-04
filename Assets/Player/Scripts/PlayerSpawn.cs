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
        Vector3 singleShotParticlePosition = transform.position + new Vector3(0f, 1.7f, 2.5f);
        GameObject singleShotObject = Instantiate(defaultStartingAbility, singleShotParticlePosition, Quaternion.identity, abilityManager.transform);
        SingleShotProjectile singleShotProjectile = singleShotObject.GetComponent<SingleShotProjectile>();
        abilityManager.AddAbility(singleShotProjectile);
    }
}
