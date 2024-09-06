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
            Logger.LogError("Missing reference to local particle system", this);
        }

        // Initialize the particles array with an initial size
        particles = new ParticleSystem.Particle[meleeSlashFX.main.maxParticles];        
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
