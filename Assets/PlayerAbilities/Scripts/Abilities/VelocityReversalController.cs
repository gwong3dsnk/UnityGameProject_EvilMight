using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityReversalController : MonoBehaviour
{
    private ParticleSystem meleeSlashFX;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        meleeSlashFX = GetComponent<ParticleSystem>();

        if (meleeSlashFX == null)
        {
            Debug.LogError("Missing reference to local particle system", this);
        }
    }

    void Update()
    {
        int numParticlesAlive = meleeSlashFX.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            if (particles[i].remainingLifetime < particles[i].startLifetime / 2)
            {
                particles[i].velocity = -particles[i].velocity;
            }

            meleeSlashFX.SetParticles(particles, numParticlesAlive);
        }

    }
}
