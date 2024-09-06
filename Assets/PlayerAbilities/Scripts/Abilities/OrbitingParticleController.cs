using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingParticleController : MonoBehaviour
{
    [SerializeField] float orbitRadius = 5f;         // Radius of the orbit
    [SerializeField] float orbitSpeed = 30f;         // Speed of the orbiting motion
    private ParticleSystem orbitingFX;  // Reference to the Particle System
    private ParticleSystem.Particle[] particles;
    private Vector3 origin;

    void Start()
    {
        orbitingFX = GetComponent<ParticleSystem>();

        // Initialize the origin point (e.g., the center of the orbit)
        origin = transform.position;

        // Get the particle array from the Particle System
        particles = new ParticleSystem.Particle[orbitingFX.main.maxParticles];
        int numParticles = orbitingFX.GetParticles(particles);

        // Set initial positions for the 3 particles in a circle
        float angleStep = 360f / numParticles;
        for (int i = 0; i < numParticles; i++)
        {
            float angle = i * angleStep;
            Vector3 position = origin + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius, 0);
            particles[i].position = position;
        }

        // Apply the particles to the Particle System
        orbitingFX.SetParticles(particles, numParticles);
    }

    void Update()
    {
        // Update particle positions to orbit around the origin
        float deltaTime = Time.deltaTime;

        int numParticles = orbitingFX.GetParticles(particles);
        for (int i = 0; i < numParticles; i++)
        {
            float angle = Mathf.Atan2(particles[i].position.y - origin.y, particles[i].position.x - origin.x) * Mathf.Rad2Deg;
            angle += orbitSpeed * deltaTime;
            Vector3 newPosition = origin + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius, 0);
            particles[i].position = newPosition;
        }

        // Apply the updated particles to the Particle System
        orbitingFX.SetParticles(particles, numParticles);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision
        // For example, stop the particle system or change particle behavior
        orbitingFX.Stop();
        Logger.Log("Collision detected! Stopping particle system.");
    }
}