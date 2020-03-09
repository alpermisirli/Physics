using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Physics2D
{
    public class Vector2D
    {
        //Coordinates of the Vector2D
        public float x { get; set; }
        public float y { get; set; }

        //Default Vector2D Constructor
        public Vector2D()
        {
            x = 0;
            y = 0;
        }

        //Vector2D Constructor
        public Vector2D(float a, float b)
        {
            x = a;
            y = b;
        }

        //Invert function for Vector2D
        void InvertVector2D()
        {
            x = -x;
            y = -y;
        }

        /// <summary>
        /// Gets the magnitude or length of this vector.
        /// </summary>
        public float MagnitudeVector2D
        {
            get { return Mathf.Sqrt(x * x + y * y); }
        }
       

        /// <summary>
        /// Gets the square of the magnitude.
        /// </summary>
        public float SquareMagnitudeVector2D
        {
            get { return x * x + y * y; }
        }

        /// <summary>
        /// Normalize this vector.
        /// </summary>
        public void NormalizeVector2D()
        {
            float length = MagnitudeVector2D;
            if (length > 0)
            {
                x *= 1 / length;
                y *= 1 / length;
            }
        }



        //void Vector2D operator *(float value)
        //{
        //    x *= value;
        //    y * = value;
        //}

        //TODO TAKE A LOOK AT THIS FUNCTION AGAIN book page 26
        /// <summary>
        /// Scale a vector by an amount.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="value">The value to multiply the vector by.</param>
        /// <returns>The resulting vector from the multiplication.</returns>
        public static Vector2D operator *(Vector2D lhs, float value)
        {
            return new Vector2D(lhs.x * value, lhs.y * value);
        }

        /// <summary>
        /// Add a scaled vector to this vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scale">The amount to scale.</param>
        public void AddScaledVector(Vector2D vector, float scale)
        {
            x += vector.x * scale;
            y += vector.y * scale;
        }
    }
}

