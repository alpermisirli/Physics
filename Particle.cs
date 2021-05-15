using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Physics2d
{
    public class Particle
    {

        public Rigidbody2D rb;
        /// <summary>
        /// Gets or sets the damping of the particle.
        /// </summary>
        /// <remarks>
        /// Damping represents a rough approximation of drag. A proportion of the
        /// objects velocty is removed at each update based on this value. A value
        /// of 1 means the object keeps all its velocity.
        /// </remarks>
        public float damping { get; set; }

        /// <summary>
        /// Gets or sets the position of the particle.
        /// </summary>
        public Vector2D position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the particle.
        /// </summary>
        public Vector2D velocity { get; set; }

        /// <summary>
        /// Gets or sets the accumulated force applied to the particle.
        /// </summary>
        protected Vector2D forceAccum { get; set; }

        /// <summary>
        /// Gets or sets the acceleration of the particle.
        /// </summary>
        public Vector2D acceleration { get; set; }

        /// <summary>
        /// Gets or sets the inverse mass of the particle.
        /// </summary>
        /// <remarks>
        /// It is more practicle to use an inverse mass as integration is simpler
        /// and it makes more sense to have an object of infinite mass i.e. on that
        /// cannot be moved.
        /// </remarks>
        public float inverseMass { get; set; }

        /// <summary>
        /// Gets or sets the mass of the particle.
        /// </summary>
        public float mass
        {
            get
            {
                if (Equals(inverseMass, 0f))
                {
                    return float.MaxValue;
                }
                return 1.0f / inverseMass;
            }

            set
            {
                // Mass cannot be zero, since force will generate infinite acceleration.
                if (Equals(value, 0f))
                {
                    throw new Exception("Mass cannot be zero");
                }

                inverseMass = 1.0f / value;
            }

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Particle"/> class.
        /// </summary>
        public Particle()
        {
            position = new Vector2D();
            velocity = new Vector2D();
            forceAccum = new Vector2D();
            acceleration = new Vector2D();
        }

        /// <summary>
        /// Determines if this particle has finite mass.
        /// </summary>
        /// <returns><c>true</c> if particle has finite mass; otherwise, <c>false</c>.</returns>
        public bool HasFiniteMass()
        {
            return inverseMass >= 0.0;
        }

        /// <summary>
        /// Set the velocity of this particle.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public void SetVelocity(float x, float y)
        {
            velocity.x = x;
            velocity.y = y;
        }

        /// <summary>
        /// Set the position of this particle.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public void SetPosition(float x, float y)
        {
            position.x = x;
            position.y = y;
        }

        /// <summary>
        /// Set the acceleration of this particle.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public void SetAcceleration(float x, float y)
        {
            acceleration.x = x;
            acceleration.y = y;
        }

        /// <summary>
        /// Get a copy of the particle position.
        /// </summary>
        /// <returns>A copy of the particle position vector.</returns>
        public Vector2D GetPosition()
        {
            return new Vector2D(position.x, position.y);
        }

        /// <summary>
        /// Get a copy of the particle velocity.
        /// </summary>
        /// <returns>A copy of the particle velocity vector.</returns>
        public Vector2D GetVelocity()
        {
            return new Vector2D(velocity.x, velocity.y);
        }

        /// <summary>
        /// Get a copy of the particle acceleration.
        /// </summary>
        /// <returns>A copy of the particle acceleration vector.</returns>
        public Vector2D GetAcceleration()
        {
            return new Vector2D(acceleration.x, acceleration.y);
        }

        /// <summary>
        /// Calculate the new position and velocity of the particle.
        /// </summary>
        /// <param name="duration">
        /// Time interval over which to update the position and velocity.
        /// This is currently the time between frames.
        /// </param>
        public void Integrate(float duration)
        {
            // We don't integrate things with zero mass.
            if (inverseMass <= 0.0f)
            {
                return;
            }

            // Make sure duration is positive.
            if (duration <= 0.0)
            {
                throw new ArgumentOutOfRangeException("duration", "must be greater than 0");
            }

            // Update linear position.
            position.AddScaledVector(velocity, duration);

            // Work out the acceleration from the force.
            Vector2D resultingAcc = GetAcceleration();
            resultingAcc.AddScaledVector(forceAccum, inverseMass);

            // Update linear velocity from the acceleration.
            velocity.AddScaledVector(resultingAcc, duration);

            // Impose drag.
            velocity *= Mathf.Pow(damping, duration);

            // Clear the forces.
            ClearAccumulator();
        }

        /// <summary>
        /// Add a force to this particle.
        /// </summary>
        /// <param name="force">The force to add.</param>
        public void AddForce(Vector2D force)
        {
            force += force;
        }

        /// <summary>
        /// Clear the accumulated force.
        /// </summary>
        public void ClearAccumulator()
        {
            forceAccum.Clear();
        }
    }
}



