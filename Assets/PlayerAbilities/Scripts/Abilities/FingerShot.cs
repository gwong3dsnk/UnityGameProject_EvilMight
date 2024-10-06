using System;
using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FingerShot : PlayerAbilities
{
    private AbilityHelperData[] abilityHelperData;
    private GameObject renderMesh; 
    private ParticleSystem[] particleSystems; 
    private bool isSwitchingFX = true;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActivateAbility(PlayerAbilities ability)
    {
        abilityHelperData = GetComponentInParent<AbilityHelper>().AbilityHelperData;

        if (abilityHelperData == null)
        {
            Logger.LogError("[FingerShot] - Missing reference to AbilityHelper component.", this);
        }

        EnableVariantHandMeshes();
        playerSockets = abilityHelperData[1].meshSockets[0].meshSockets;
        HandleAbilityParticleSystem(ability);
        Logger.Log("[FingerShot] - Invoking event to play animation.", this);
        PlayerAbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 
    }

    /// <summary>
    /// SetActive the FingerShot hands and ensure it adopts the same position/rotation as the default large hands.
    /// [0] is the default large hands, [1] is the FingerShot small hands that are close together.
    /// </summary>
    private void EnableVariantHandMeshes()
    {
        renderMesh = abilityHelperData[1].meshSockets[0].renderMesh;
        renderMesh.SetActive(true);
        renderMesh.transform.position = renderMesh.transform.position;
        renderMesh.transform.rotation = renderMesh.transform.rotation;
    }

    private void HandleAbilityParticleSystem(PlayerAbilities ability)
    {
        GetAbilityParticleSystem(ability);
        SetParticleSystemTransforms();
    }

    private void GetAbilityParticleSystem(PlayerAbilities ability)
    {
        // Retrieve the runtime gameobject's particle system component.
        particleSystems = ability.gameObject.GetComponentsInChildren<ParticleSystem>();
        if (particleSystems.Length == 0)
        {
            Logger.LogWarning("[FingerShot] - No particle system gameobject components found.", this);
        }
    }            

    private void SetParticleSystemTransforms()
    {
        // Set transforms for particle system in left index socket
        particleSystems[0].transform.position = playerSockets[0].transform.position;
        particleSystems[0].transform.rotation = renderMesh.transform.rotation;
        // Set transforms for particle system in right index socket
        particleSystems[1].transform.position = playerSockets[1].transform.position;
        particleSystems[1].transform.rotation = renderMesh.transform.rotation;
    }

    public override void HandlePlayAnimEventFX()
    {
        Logger.Log("[FingerShot] - Attempting to play right and left particle systems.", this);
        if (isSwitchingFX)
        {
            PlayRightParticleSystem();
        }
        else
        {
            PlayLeftParticleSystem();
        }
    }    

    public void PlayLeftParticleSystem()
    {
        // Called by FingerShot animation event
        if (particleSystems[0].isPlaying)
        {
            particleSystems[0].Stop();
        }

        particleSystems[0].Play(); 
        isSwitchingFX = true;
    }

    public void PlayRightParticleSystem()
    {
        // Called by FingerShot animation event
        if (particleSystems[1].isPlaying)
        {
            particleSystems[1].Stop();
        }

        particleSystems[1].Play();   
        isSwitchingFX = false;
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
    }

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }

    protected override void InitializeAbilityData()
    {
        base.InitializeAbilityData();
    }
}
