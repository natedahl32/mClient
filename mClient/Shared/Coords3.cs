using System.Globalization;

namespace mClient.Shared
{
    /// <summary>
    ///  Represents a coordinates of WoW object without orientation.
    /// </summary>
    public class Coords3
    {
        public float X;
        public float Y;
        public float Z;

        /// <summary>
        ///  Converts the numeric values of this instance to its equivalent string representations, separator is space.
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Coords3;
            if (other == null) return false;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public static Coords3 Zero()
        {
            return new Coords3();
        }
    }
}
