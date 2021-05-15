using System.Collections;
using System.Collections.Generic;
using Physics2d;
using UnityEngine;

public class Artillery : MonoBehaviour
{
    #region Unity Editor

    /// <summary>
    /// Mass of the particle.
    /// </summary>
    public float mass = 200f;

    /// <summary>
    ///  The velocity of the particle.
    /// </summary>
    public Vector2D velocity;

    /// <summary>
    /// The acceleration of the particle.
    /// </summary>
    public Vector2D acceleration;

    /// <summary>
    /// The damping of the particle.
    /// </summary>
    public float damping;

    #endregion

    /// <summary>
    /// Create a particle instance.
    /// </summary>
    private Physics2d.Particle particle = new Physics2d.Particle();

    /// <summary>
    /// Set the default properties of the particle.
    /// </summary>
    private void Start()
    {
        particle.mass = mass;
        particle.SetPosition(transform.position.x, transform.position.y);
        particle.SetVelocity(velocity.x, velocity.y);
        particle.SetAcceleration(acceleration.x, acceleration.y);
        particle.damping = damping;
        //SetObjectPosition(particle.position);
        transform.position = particle.position;
    }

    /// <summary>
    /// Update the particle position.
    /// </summary>
    private void FixedUpdate()
    {
        particle.Integrate(Time.deltaTime);
        transform.position = particle.position;
        //SetObjectPosition(particle.Position);
    }

}
