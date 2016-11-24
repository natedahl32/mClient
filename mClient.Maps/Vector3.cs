using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Maps
{
    public class Vector3
    {
        #region Constructors

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Properties

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        #endregion

        #region Public Methods

        public float squaredLength()
        {
            return squaredMagnitude();
        }

        public float squaredMagnitude()
        {
            return X * X + Y * Y + Z * Z;
        }

        #endregion

        #region Operator Overloads

        public static Vector3 operator -(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z);
        }

        public static Vector3 operator +(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public static Vector3 operator *(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.X * c2.X, c1.Y * c2.Y, c1.Z * c2.Z);
        }

        public static Vector3 operator /(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.X / c2.X, c1.Y / c2.Y, c1.Z / c2.Z);
        }

        public static bool operator ==(Vector3 c1, Vector3 c2)
        {
            return c1.X == c2.X && c1.Y == c2.Y && c1.Z == c2.Z;
        }

        public static bool operator !=(Vector3 c1, Vector3 c2)
        {
            return c1.X != c2.X || c1.Y != c2.Y || c1.Z != c2.Z;
        }

        #endregion
    }
}
