using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class FistSlam : AbilityBase
{
    private Coroutine attackCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    #region Public Methods
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        isAttacking = true;
        particleSystems[0].gameObject.SetActive(true);
        attackCoroutine = StartCoroutine(FistSlamAttackCoroutine());    
    }

    public override void DeactivateAbility()
    {
        base.DeactivateAbility();
        isAttacking = false;
        if (attackCoroutine != null) attackCoroutine = null;
    }    

    public override void ActivateUpgrade(UpgradeTypesDatabase newUpgrade)
    {
        base.ActivateUpgrade(newUpgrade);
    }    

    public override void HandleAnimEventFX()
    {
        if (particleSystems.Length == 0) 
        {
            Logger.LogWarning("[FingerFlick] - No particle system gameobject components found.", this);
        }
        else
        {
            particleSystems[0].transform.position = player.position;
            PlayParticleSystem();
        }
    }

    public override void UpgradeActivationDelay(float upgradeValue)
    {
        attackSpeed = upgradeValue;
    }    
    #endregion

    #region Private Methods
    private IEnumerator FistSlamAttackCoroutine()
    {
        while (isAttacking)
        {
            AbilitiesManager.AbilityManagerInstance.InvokeHandleAbilityPlayAnimEvent(this); 

            yield return new WaitForSeconds(1.0f); // Let anim & vfx play before damaging enemies.

            Dictionary<Collider, float> enemiesInRange = base.GetNearbyEnemies(transform.position, abilityRange);

            foreach (KeyValuePair<Collider, float> enemy in enemiesInRange)
            {
                EnemyHealth enemyHealth = enemy.Key.GetComponent<EnemyHealth>();
                enemyHealth.TakeGeneralDamage(this);
            }

            yield return new WaitForSeconds(attackSpeed);
        }
    }

    private void PlayParticleSystem()
    {
        if (particleSystems[0].isPlaying) particleSystems[0].Stop();
        
        particleSystems[0].Play();
    }
    #endregion    
}
