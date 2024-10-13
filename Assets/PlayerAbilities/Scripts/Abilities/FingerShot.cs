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
    private ParticleSystem[] particleSystems; 
    private bool isSwitchingFX = true;

    protected override void Awake()
    {
        base.Awake();
    }

    #region Public Methods
    public override void ActivateAbility()
    {
        helperData = GetComponentInParent<AbilityHelper>().AbilityHelperData.FirstOrDefault(data => data.abilityNames == AbilityNames.FingerShot);

        if (helperData == null)
        {
            Logger.LogError($"[{this.name}] - Missing reference to AbilityHelper component.", this);
        }

        EnableVariantHandMeshes();
        base.ActivateAbility();
        playerSockets = helperData.meshSockets[0].meshSockets;
        HandleAbilityParticleSystem();
        AbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
    }

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }    

    public override void UpgradeActivationDelay(float upgradeValue)
    {
        Logger.Log($"[{this.name}] - No need to update activationDelay.", this);
    }    

    public void PlayLeftParticleSystem()
    {
        Logger.Log($"[{this.name}] - 3. Playing LEFT PARTICLE for [{this.name}] animation.", this);
        if (particleSystems[0].isPlaying)
        {
            particleSystems[0].Stop();
        }

        particleSystems[0].Play(); 
        isSwitchingFX = true;
    }

    public void PlayRightParticleSystem()
    {
        Logger.Log($"[{this.name}] - 3. Playing RIGHT PARTICLE for [{this.name}] animation.", this);
        if (particleSystems[1].isPlaying)
        {
            particleSystems[1].Stop();
        }

        particleSystems[1].Play();   
        isSwitchingFX = false;
    }      

    public override void HandleAnimEventFX() 
    {
        Logger.Log($"[{this.name}] - 2. STARTING play right and left PARTICLE for [{this.name}] animation.", this);
        if (isSwitchingFX)
        {
            PlayRightParticleSystem();
        }
        else
        {
            PlayLeftParticleSystem();
        }
    }           
    #endregion

    #region Protected Methods
    protected override void InitializeAbilityData()
    {
        base.InitializeAbilityData();
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

    private void HandleAbilityParticleSystem()
    {
        GetAbilityParticleSystem();
        SetParticleSystemTransforms();
    }

    private void GetAbilityParticleSystem()
    {
        // Retrieve the runtime gameobject's particle system component.
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        if (particleSystems.Length == 0)
        {
            Logger.LogWarning($"[{this.name}] - No particle system gameobject components found.", this);
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
    #endregion
}
