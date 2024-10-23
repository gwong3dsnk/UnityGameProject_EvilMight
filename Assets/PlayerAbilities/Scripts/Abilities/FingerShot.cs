using System.Linq;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FingerShot : AbilityBase
{
    private AbilityHelperData helperData;
    private GameObject renderMesh; 
    private bool isSwitchingFX = true;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        if (particleSystems.Length > 1)
        {
            SetParticleSystemTransforms();
        }
    }    

    #region Public Methods
    public override void ActivateAbility()
    {
        helperData = GetComponentInParent<AbilityHelper>().AbilityHelperData.FirstOrDefault(data => data.abilityNames == AbilityNames.FingerShot);

        if (helperData == null)
        {
            Logger.LogError($"[{this.name}] - Missing reference to AbilityHelper component.", this);
            return;
        }

        EnableVariantHandMeshes();
        base.ActivateAbility();
        playerSockets = helperData.meshSockets[0].meshSockets;
        GetAbilityParticleSystems();

        AbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();

        foreach (ParticleSystem particleFX in particleSystems)
        {
            ParticleSystem.EmissionModule emissionModule = particleFX.emission;
            emissionModule.enabled = false;
        }
    }

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }    

    public override void UpgradeActivationDelay(float upgradeValue)
    {
        // No need to update activationDelay.
    }    

    public void PlayLeftParticleSystem()
    {
        if (particleSystems[0].isPlaying) particleSystems[0].Stop();

        particleSystems[0].Play(); 
        isSwitchingFX = true;
    }

    public void PlayRightParticleSystem()
    {
        if (particleSystems[1].isPlaying) particleSystems[1].Stop();

        particleSystems[1].Play();
        isSwitchingFX = false;
    }      

    public override void HandleAnimEventFX() 
    {
        if (isSwitchingFX) PlayRightParticleSystem();
        else PlayLeftParticleSystem();
    }           
    #endregion

    #region Private Methods
    /// <summary>
    /// SetActive the FingerShot hands and ensure it adopts the same position/rotation as the default large hands.
    /// [0] is the default large hands, [1] is the FingerShot small hands that are close together.
    /// </summary>
    private void EnableVariantHandMeshes()
    {
        renderMesh = helperData.meshSockets[0].renderMesh;
        renderMesh.SetActive(true);
        renderMesh.transform.position = renderMesh.transform.position;
        renderMesh.transform.rotation = renderMesh.transform.rotation;
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

    protected virtual void GetAbilityParticleSystems()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        if (particleSystems.Length == 0)
        {
            Logger.LogWarning($"[{this.name}] - No particle systems found.", this);
        }
    }    
    #endregion
}
