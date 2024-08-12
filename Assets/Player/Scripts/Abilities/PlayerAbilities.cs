using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbilities : MonoBehaviour
{
    [SerializeField] protected ParticleSystem abilityParticleSystem;
    [SerializeField] protected int damage;
    public int Damage => damage;
    [SerializeField] protected int speedMultiplier;

    public virtual void ActivateAbility()
    {
        if (abilityParticleSystem != null)
        {
            abilityParticleSystem.Play();
        }
    }

    public virtual void DeactivateAbility()
    {
        if (abilityParticleSystem != null)
        {
            abilityParticleSystem.Stop();
        }
    }

    public abstract void UpgradeAbility();
}
