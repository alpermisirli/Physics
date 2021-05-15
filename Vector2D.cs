using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Physics2d
{
    //I may remove serializable part later
    [System.Serializable]
    public class Vector2D
    {
        //Coordinates of the Vector2D
        public float x;
        public float y;
        //removed getter and setter

        //Default Vector2D Constructor
        public Vector2D()
        {
            x = 0;
            y = 0;
        }

        //todo i may remove
        /// <summary>
        ///   <para>Set x and y components of an existing Vector2.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void Set(float newX, float newY)
        {
            this.x = newX;
            this.y = newY;
        }
        //todo i may remove

        //Vector2D Constructor
        public Vector2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Vector2D"/> class.
        /// </summary>
        /// <param name="vector">The vector to copy.</param>
        public Vector2D(Vector2D vector)
        {
            x = vector.x;
            y = vector.y;
        }
        private static readonly Vector2D zeroVector = new Vector2D(0.0f, 0.0f);
        private static readonly Vector2D oneVector = new Vector2D(1f, 1f);
        private static readonly Vector2D upVector = new Vector2D(0.0f, 1f);
        private static readonly Vector2D downVector = new Vector2D(0.0f, -1f);
        private static readonly Vector2D leftVector = new Vector2D(-1f, 0.0f);
        private static readonly Vector2D rightVector = new Vector2D(1f, 0.0f);
        private static readonly Vector2D gravityVector = new Vector2D(0f, -10.0f);

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 0).</para>
        /// </summary>
        public static Vector2D zero
        {
            get
            {
                return Vector2D.zeroVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 1).</para>
        /// </summary>
        public static Vector2D one
        {
            get
            {
                return Vector2D.oneVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 1).</para>
        /// </summary>
        public static Vector2D up
        {
            get
            {
                return Vector2D.upVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, -1).</para>
        /// </summary>
        public static Vector2D down
        {
            get
            {
                return Vector2D.downVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(-1, 0).</para>
        /// </summary>
        public static Vector2D left
        {
            get
            {
                return Vector2D.leftVector;
            }
        }

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 0).</para>
        /// </summary>
        public static Vector2D right
        {
            get
            {
                return Vector2D.rightVector;
            }
        }

        public static Vector2D gravity
        {
            get { return Vector2D.gravityVector; }
        }

        //Invert function for Vector2D
        public void Invert()
        {
            x = -x;
            y = -y;
        }

        /// <summary>
        /// Gets the magnitude or length of this vector.
        /// </summary>
        public float Magnitude
        {
            get { return Mathf.Sqrt(x * x + y * y); }
        }

        /// <summary>
        /// Normalize this vector.
        /// </summary>
        public void Normalize()
        {
            float length = Magnitude;
            if (length > 0)
            {
                x *= 1 / length;
                y *= 1 / length;
            }
        }


        /// <summary>
        /// Gets the square of the magnitude.
        /// </summary>
        public float SquareMagnitude
        {
            get { return x * x + y * y; }
        }

        //addition between two vectors and a float value
        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x + b.x, a.y + b.y);
        }
        public static Vector2D operator +(Vector2D a, float b)
        {
            return new Vector2D(a.x + b, a.y + b);
        }
        public static Vector2D operator +(float b, Vector2D a)
        {
            return new Vector2D(a.x + b, a.y + b);
        }

        //Substraction between vectors and float value
        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x - b.x, a.y - b.y);
        }
        public static Vector2D operator -(Vector2D a, float b)
        {
            return new Vector2D(a.x - b, a.y - b);
        }
        public static Vector2D operator -(float b, Vector2D a)
        {
            return new Vector2D(b - a.x, b - a.y);
        }
        //inverting a vector2d
        public static Vector2D operator -(Vector2D a)
        {
            return new Vector2D(-a.x, -a.y);
        }
        //TODO NAME THIS PROPERLY
        //TODO NAME THIS PROPERLY

        //Scale the vector by an amount
        public static Vector2D operator *(Vector2D a, float d)
        {
            return new Vector2D(a.x * d, a.y * d);
        }
        public static Vector2D operator *(float d, Vector2D a)
        {
            return new Vector2D(a.x * d, a.y * d);
        }

        /// <summary>
        /// Calculate the dot product.
        /// </summary>
        /// <param name="lhs">The left vector.</param>
        /// <param name="rhs">The right vector.</param>
        /// <returns>The dot product.</returns>
        public static float operator *(Vector2D lhs, Vector2D rhs)
        {
            return (lhs.x * rhs.x) + (lhs.y * rhs.y);
        }

        //Division
        public static Vector2D operator /(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x / b.x, a.y / b.y);
        }
        public static Vector2D operator /(Vector2D a, float d)
        {
            return new Vector2D(a.x / d, a.y / d);
        }

        //Unity transform is in Vector3. How can I make my Vector2D compatible? 
        //with the unity's transform
        public static implicit operator Vector3(Vector2D v)
        {
            return new Vector2(v.x, v.y);
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

        /// <summary>
        /// Calculate the component product.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>A new vector representing the component product.</returns>
        public Vector2D ComponentProduct(Vector2D vector)
        {
            return new Vector2D(x * vector.x, y * vector.y);
        }

        /// <summary>
        /// Calculate the component product and update this vector with the result.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public void ComponentProductUpdate(Vector2D vector)
        {
            x *= vector.x;
            y *= vector.y;
        }

        //TODO CROSS PRODUCT HAKKINDA KARAR VER ARAŞTIR
        public float CrossProduct(Vector2D a, Vector2D b)
        {
            return (a.x * b.y) - (a.y * b.x);
        }

        //TODO ORTHONORMAL BASIS

        /// <summary>
        /// Calculate the dot product.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The dot product.</returns>
        public float ScalarProduct(Vector2D vector)
        {
            return (x * vector.x) + (y * vector.y);
        }

        /// <summary>
        /// Clear this vector to (0, 0).
        /// </summary>
        public void Clear()
        {
            x = y = 0;
        }

        public static Vector2D RandomVector(Vector2D min, Vector2D max)
        {
            return new Vector2D(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }
    }

}

