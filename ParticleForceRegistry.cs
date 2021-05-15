﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Physics2d;
// Create an alias for the pair within this file only.
using ParticleForceRegistration = System.Collections.Generic.KeyValuePair<Physics2d.Particle, Physics2d.IParticleForceGenerator>;

/// <summary>
/// Class for registering a particle with associated force generator.
/// </summary>
/// <example>
/// 
///     // Create the particle force registry.
///     private ParticleForceRegistry pfr = new ParticleForceRegistry();
/// 
///     // Add the particle and the particle force.
///     ParticleGravity particleGravity = new ParticleGravity(new Vector2D(0.0f, -9.8f));
///     pfr.Add(particles.particle, particleGravity);
/// 
///     // Update forces and integrate the objects.
///     pfr.UpdateForces(duration);
///     particles.particle.Integrate(duration);
/// 
/// </example>
namespace Physics2d
{
    public class ParticleForceRegistry
    {
        /// <summary>
        /// List of registered particles.
        /// </summary>
        private List<ParticleForceRegistration> registrations;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleForceRegistry"/> class.
        /// </summary>
        public ParticleForceRegistry()
        {
            registrations = new List<ParticleForceRegistration>();
        }

        /// <summary>
        /// Add a particle and the associated force generator.
        /// </summary>
        /// <param name="particle">The particle.</param>
        /// <param name="fg">The force generator.</param>
        public void Add(Particle particle, IParticleForceGenerator fg)
        {
            ParticleForceRegistration pair = new ParticleForceRegistration(particle, fg);
            registrations.Add(pair);
        }

        /// <summary>
        /// Remove a particle and the associated force generator.
        /// </summary>
        /// <param name="particle">The particle.</param>
        /// <param name="fg">The force generator.</param>
        public void Remove(Particle particle, IParticleForceGenerator fg)
        {
            registrations.Remove(new KeyValuePair<Particle, IParticleForceGenerator>(particle, fg));
        }

        /// <summary>
        /// Clear the list of registered particles.
        /// </summary>
        public void Clear()
        {
            registrations.Clear();
        }

        /// <summary>
        /// Update the particles with their respective forces.
        /// </summary>
        /// <param name="duration"></param>
        public void UpdateForces(float duration)
        {
            foreach (ParticleForceRegistration registration in registrations)
            {
                registration.Value.UpdateForce(registration.Key, duration);
                //TODO TAKE A LOOK AT THIS IN MORE DETAIL
            }
        }
    }

}
