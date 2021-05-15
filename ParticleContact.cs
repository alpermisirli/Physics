﻿using System;
using Unity;

namespace Physics2d
{
    /// <summary>
    /// A contact represents two objects in contact (in this case
    /// ParticleContact representing two particles). Resolving a contact
    /// removes their interpenetration, and applies sufficient impulse to
    /// keep them apart. Colliding bodies may also rebound.
    /// 
    /// The contact has no callable methods, it just hold the contact
    /// details. To resolve a set of contacts, use the particle contact
    /// resolver class.
    /// </summary>
    public class ParticleContact
    {
        /// <summary>
        /// Holds the particles that are involved in the contact. The second of
        /// these can be null for contacts with the scenery.
        /// </summary>
        public Particle[] particle;

        public Vector2D[] ParticleMovement;

        public ParticleContact()
        {
            particle = new Particle[2];
            ParticleMovement = new Vector2D[2];
            ParticleMovement[0] = new Vector2D();
            ParticleMovement[1] = new Vector2D();
        }

        /// <summary>
        /// Holds the normal restitution coefficient at the contact. //TODO Study this more!
        /// </summary>
        public float Restitution { get; set; }

        /// <summary> //TODO STUDY THIS MORE!
        /// Holds the direction of the contact in world coordinates.
        /// </summary>
        public Vector2D ContactNormal { get; set; }

        /// <summary>
        /// Holds the depth of the penetration at the contact.
        /// </summary>
        public float Penetration { get; set; }

        /// <summary>
        /// Resolves this contact for both velocity and interpenetration.
        /// </summary>
        /// <param name="duration">Time interval over which to calculate velocity and interpenetration.</param>
        public void Resolve(float duration)
        {
            ResolveVelocity(duration);
            ResolveInterpenetration(duration);
        }

        /// <summary>
        /// Calculates the separating velocity at this contact.
        /// </summary>
        /// <returns>The separating velocity.</returns>
        public float CalculateSeparatingVelocity()
        {
            // Calculate the relative velocity.
            Vector2D relativeVelocity = particle[0].GetVelocity();
            if (particle[1] != null)
            {
                relativeVelocity -= particle[1].GetVelocity();
            }

            // Return the dot product of velocity and contact normal.
            return relativeVelocity * ContactNormal;
        }

        /// <summary>
        /// Handles the impulse calculations for this collision.
        /// </summary>
        /// <param name="duration">Time interval over which to update the impulse.</param>
        private void ResolveVelocity(float duration)
        {
            // Find the velocity in the direction of the contact.
            float separatingVelocity = CalculateSeparatingVelocity();

            // Check if it needs to be resolved.
            if (separatingVelocity > 0)
            {
                // The contact is either separating or stationary;
                // there's no impulse required.
                return;
            }

            // Calculate the new separating velocity.
            float newSepVelocity = -separatingVelocity * Restitution;

            // Check the velocity buildup due to acceleration only.
            Vector2D accCausedVelocity = particle[0].GetAcceleration();

            if (particle[1] != null)
            {
                accCausedVelocity -= particle[1].GetAcceleration();
            }

            float accCausedSepVelocity = accCausedVelocity * ContactNormal * duration;

            // If there is a closing velocity due to acceleration buildup,
            // remove it from the separating velocity.
            if (accCausedSepVelocity < 0)
            {
                newSepVelocity += Restitution * accCausedSepVelocity;

                if (newSepVelocity < 0)
                {
                    newSepVelocity = 0;
                }
            }

            float deltaVelocity = newSepVelocity - separatingVelocity;

            // We apply the change in velocity to each object in proportion
            // to their inverse mass (those with lower inverse mass get less
            // change in velocity).
            float totalInverseMass = particle[0].inverseMass;

            if (particle[1] != null)
            {
                totalInverseMass += particle[1].inverseMass;
            }

            // If all particles have infinite mass, then impulses have no effect.
            if (totalInverseMass <= 0)
            {
                return;
            }

            // Calculate the impulse to apply.
            float impulse = deltaVelocity / totalInverseMass;

            // Find the amount of impulse per unit of inverse mass.
            Vector2D impulsePerIMass = ContactNormal * impulse;

            // Apply impulses: they are applied in the direction of the contact,
            // and ae propertional to inverse mass.
            particle[0].velocity += impulsePerIMass * particle[0].inverseMass;

            if (particle[1] != null)
            {
                // Particle 1 goes in the opposite direction.
                particle[1].velocity += impulsePerIMass * -particle[1].inverseMass;
            }
        }

        /// <summary>
        /// Handles the interpenetration resolution for this contact.
        /// </summary>
        /// <param name="duration">Time interval over which to update the interpenetration.</param>
        private void ResolveInterpenetration(float duration)
        {
            // If there is no penetration, nothing to do.
            if (Penetration <= 0)
            {
                return;
            }

            // The movement of each object is based on their inverse mass.
            float totalInverseMass = particle[0].inverseMass;

            if (particle[1] != null)
            {
                totalInverseMass += particle[1].inverseMass;
            }

            // If all particles have infinite mass, nothing to do.
            if (totalInverseMass <= 0)
            {
                return;
            }

            // Find the amount of penetration resolution per unit of inverse mass.
            Vector2D movePerIMass = ContactNormal * (Penetration / totalInverseMass);

            // Calculate the movement amounts.
            ParticleMovement[0] = movePerIMass * particle[0].inverseMass;

            if (particle[1] != null)
            {
                ParticleMovement[1] = movePerIMass * -particle[1].inverseMass;
            }

            // Apply the penetration resolution.
            particle[0].position += ParticleMovement[0];

            if (particle[1] != null)
            {
                particle[1].position += ParticleMovement[1];
            }
        }
    }

    /// <summary>
    /// The contact resolution implementation for particle contacts.
    /// One resolver instance can be shared for the entire simulation.
    /// </summary>
    public class ParticleContactResolver
    {
        /// <summary>
        /// Gets or sets the number of iterations allowed.
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// This is a performance tracking value; we keep track of the
        /// number of iterations used.
        /// </summary>
        protected int IterationsUsed { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ParticleContactResolver"/> class.
        /// </summary>
        /// <param name="iterations">The number of iterations allowed.</param>
        public ParticleContactResolver(int iterations)
        {
            Iterations = iterations;
        }

        /// <summary>
        /// Resolves a set of particle contacts for both penetration and velocity.
        /// TODO: Revisit the method arguments, we could possibly use a list.
        /// </summary>
        /// <param name="contactArray">The set of particle contacts.</param>
        /// <param name="numberOfContacts">The number of contacts.</param>
        /// <param name="duration">Time interval over which to resolve the contacts.</param>
        public void ResolveContacts(ParticleContact[] contactArray, int numberOfContacts, float duration)
        {
            IterationsUsed = 0;

            while (IterationsUsed < Iterations)
            {
                // Find the contact with the largest closing separating velocity.
                float max = float.MaxValue;
                int maxIndex = numberOfContacts;

                for (int i = 0; i < numberOfContacts; ++i)
                {
                    float sepVelocity = contactArray[i].CalculateSeparatingVelocity();

                    if ((sepVelocity < max) && (sepVelocity < 0 || contactArray[i].Penetration > 0))
                    {
                        max = sepVelocity;
                        maxIndex = i;
                    }
                }

                // Do we have anything worth resolving?
                if (maxIndex == numberOfContacts)
                {
                    break;
                }

                // Resolve this contact.
                contactArray[maxIndex].Resolve(duration);

                // Update the interpenetrations for all particles.
                Vector2D[] move = contactArray[maxIndex].ParticleMovement;
                for (int i = 0; i < numberOfContacts; i++)
                {
                    if (contactArray[i].particle[0] == contactArray[maxIndex].particle[0])
                    {
                        contactArray[i].Penetration -= move[0] * contactArray[i].ContactNormal;
                    }
                    else if (contactArray[i].particle[0] == contactArray[maxIndex].particle[1])
                    {
                        contactArray[i].Penetration -= move[1] * contactArray[i].ContactNormal;
                    }
                    if (contactArray[i].particle[1] != null)
                    {
                        if (contactArray[i].particle[1] == contactArray[maxIndex].particle[0])
                        {
                            contactArray[i].Penetration += move[0] * contactArray[i].ContactNormal;
                        }
                        else if (contactArray[i].particle[1] == contactArray[maxIndex].particle[1])
                        {
                            contactArray[i].Penetration += move[1] * contactArray[i].ContactNormal;
                        }
                    }
                }

                ++IterationsUsed;
            }

        }
    }
}