using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Physics2d;

namespace Physics2d
{
    /// <summary>
    /// A force generator that applies a gravitational force to the particle.
    /// </summary>
    public class ParticleGravity : IParticleForceGenerator
    {
        /// <summary>
        /// The acceleration due to gravity.
        /// </summary>
        private Vector2D gravity; //TODO Actually I have a gravity Vector2D maybe use in the future?

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleGravity"/> class.
        /// </summary>
        /// <param name="gravity">The acceleration due to gravity.</param>
        public ParticleGravity(Vector2D gravity)
        {
            this.gravity = gravity;
        }

        /// <summary>
        /// Apply a gravitational force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, float duration)
        {
            // Do not handle particles with infinite mass.
            if (!particle.HasFiniteMass())
            {
                return;
            }

            // Apply the mass-scaled force to the particle.
            particle.AddForce(gravity * particle.mass);
        }
    }

    /// <summary>
    /// A force generator that applies a drag force to the particle.
    /// </summary>
    public class ParticleDrag : IParticleForceGenerator
    {
        /// <summary>
        /// The velocity drag coefficient.
        /// </summary>
        private float k1;

        /// <summary>
        /// The velocity squared drag coefficient.
        /// </summary>
        private float k2;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleDrag"/> class.
        /// </summary>
        /// <param name="k1">The velocity drag coefficient.</param>
        /// <param name="k2">The velocity squared drag coefficient.</param>
        public ParticleDrag(float k1, float k2)
        {
            this.k1 = k1;
            this.k2 = k2;
        }

        /// <summary>
        /// Apply a drag force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, float duration)
        {
            Vector2D force = particle.velocity;

            // Calculate the total drag coefficient.
            float dragCoeff = force.Magnitude;
            dragCoeff = k1 * dragCoeff + k2 * dragCoeff * dragCoeff;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= -dragCoeff;
            particle.AddForce(force);
        }
    }

    /// <summary>
    /// A basic spring force generator calculating the length of the
    /// spring and using Hook's law to calculate the force.
    /// </summary>
    public class ParticleSpring : IParticleForceGenerator
    {
        /// <summary>
        /// The particle at the other end of the spring.
        /// </summary>
        private Particle other;

        /// <summary>
        /// A value that gives the stiffness of the spring.
        /// </summary>
        private float springConstant;

        /// <summary>
        /// The natural length of the spring when no forces are acting upon it.
        /// </summary>
        private float restLength;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleSpring"/> class.
        /// </summary>
        /// <param name="other">The particle at the other end of the spring.</param>
        /// <param name="springConstant">A value that gives the stiffness of the spring.</param>
        /// <param name="restLength">The natural length of the spring when no forces are acting upon it.</param>
        public ParticleSpring(Particle other, float springConstant, float restLength)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            this.other = other;
            this.springConstant = springConstant;
            this.restLength = restLength;
        }

        /// <summary>
        /// Apply a spring force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, float duration)
        {
            // Calculate the vector of the spring.
            Vector2D force = particle.position;
            force -= other.position;

            // Calculate the magnitude of the force.
            float magnitude = force.Magnitude;
            // TODO: Not sure why this Abs calculation is used here.
            // If the distance between the two particles is less than the restLength,
            // the particles have a force which pulls them together. I would have expected
            // the two particles to push apart.
            //magnitude = System.Math.Abs(magnitude - restLength);
            magnitude -= restLength;
            magnitude *= springConstant;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= -magnitude;
            particle.AddForce(force);
        }
    }

    /// <summary>
    /// A force generator that applies a spring force, where one end is
    /// attached to a fixed point in space.
    /// </summary>
    public class ParticleAnchoredSpring : IParticleForceGenerator
    {
        /// <summary>
        /// Location of the anchored end of the spring.
        /// </summary>
        private Vector2D anchor;

        /// <summary>
        /// A value that gives the stiffness of the spring.
        /// </summary>
        private float springConstant;

        /// <summary>
        /// The natural length of the spring when no forces are acting upon it.
        /// </summary>
        private float restLength;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleAnchoredSpring"/> class.
        /// </summary>
        /// <param name="anchor">Location of the anchored end of the spring.</param>
        /// <param name="springConstant">A value that gives the stiffness of the spring.</param>
        /// <param name="restLength">The natural length of the spring when no forces are acting upon it.</param>
        public ParticleAnchoredSpring(Vector2D anchor, float springConstant, float restLength)
        {
            //if (anchor == null)
            //{
            //    throw new ArgumentNullException("anchor");
            //}

            this.anchor = anchor;
            this.springConstant = springConstant;
            this.restLength = restLength;
        }

        /// <summary>
        /// Apply a spring force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, float duration)
        {
            // Calculate the vector of the spring.
            Vector2D force = particle.position;
            force -= anchor;

            // Calculate the magnitude of the force.
            float magnitude = force.Magnitude;
            magnitude = (restLength - magnitude) * springConstant;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= magnitude;

            particle.AddForce(force);
        }
    }

    /// <summary>
    /// A force generator that applies a spring force only when extended.
    /// </summary>
    public class ParticleBungee : IParticleForceGenerator
    {
        /// <summary>
        /// The particle at the other end of the spring.
        /// </summary>
        private Particle other;

        /// <summary>
        /// A value that gives the stiffness of the spring.
        /// </summary>
        private float springConstant;

        /// <summary>
        /// The natural length of the spring when no forces are acting upon it.
        /// </summary>
        private float restLength;

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleBungee"/> class.
        /// </summary>
        /// <param name="other">The particle at the other end of the spring.</param>
        /// <param name="springConstant">A value that gives the stiffness of the spring.</param>
        /// <param name="restLength">The natural length of the spring when no forces are acting upon it.</param>
        public ParticleBungee(Particle other, float springConstant, float restLength)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            this.other = other;
            this.springConstant = springConstant;
            this.restLength = restLength;
        }

        /// <summary>
        /// Apply a spring force to the particle.
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="duration">Time interval over which to update the force.</param>
        public void UpdateForce(Particle particle, float duration)
        {
            // Calculate the vector of the spring.
            Vector2D force = particle.position;
            force -= other.position;

            // Check if the bungee is compressed.
            float magnitude = force.Magnitude;
            if (magnitude <= restLength)
            {
                return;
            }

            // Calculate the magnitude of the force.
            magnitude = (restLength - magnitude) * springConstant;

            // Calculate the final force and apply it.
            force.Normalize();
            force *= magnitude;
            particle.AddForce(force);
        }
    }

    /// <summary>
    /// A force generator that applies a buoyancy force for a plane of liquid parallel to XZ plane.
    /// </summary>
    public class ParticleBuoyancy : IParticleForceGenerator
    {
        private float maxDepth;
        private float volume;
        private float waterHeight;
        private float liquidDensity;

        public ParticleBuoyancy(float maxDepth, float volume, float waterHeight, float liquidDensity = 1000.0f)
        {
            this.maxDepth = maxDepth;
            this.volume = volume;
            this.waterHeight = waterHeight;
            this.liquidDensity = liquidDensity;
        }

        public void UpdateForce(Particle particle, float duration)
        {
            // Calculate the submersion depth.
            float depth = particle.position.y;

            // Make sure we are not out of the water.
            if (depth >= waterHeight + maxDepth)
            {
                return;
            }

            Vector2D force = new Vector2D();

            // Check if we are at maximum depth.
            if (depth <= waterHeight - maxDepth)
            {
                force.y = liquidDensity * volume;
                particle.AddForce(force);
                return;
            }

            // Otherwise we are partly submerged.
            force.y = liquidDensity * volume * (depth - maxDepth - waterHeight) / 2 * maxDepth;
            particle.AddForce(force);
        }
    }
}
