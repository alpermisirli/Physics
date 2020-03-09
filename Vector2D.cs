using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Physics2d
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
        public Vector2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Vector3"/> class.
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

        //Invert function for Vector2D
        public void InvertVector2D()
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


        /// <summary>
        /// Gets the square of the magnitude.
        /// </summary>
        public float SquareMagnitudeVector2D
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
        //Dot product
        public static Vector2D operator *(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x * b.x, a.y * b.y);
        }
        //Scale the vector by an amount
        public static Vector2D operator *(Vector2D a, float d)
        {
            return new Vector2D(a.x * d, a.y * d);
        }
        public static Vector2D operator *(float d, Vector2D a)
        {
            return new Vector2D(a.x * d, a.y * d);
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

        /// <summary>
        /// Calculate the dot product.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The dot product.</returns>
        public double ScalarProduct(Vector2D vector)
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
    }


    //TODO Testler
    public class Test
    {
        public void test()
        {
            Vector2D x = new Vector2D();

        }
    }
}

