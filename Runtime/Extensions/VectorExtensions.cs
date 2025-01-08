using UnityEngine;

namespace Utilities.Vectors
{
    public static class VectorExtensions
    {
        public static bool IsApproximately(this Vector2 a, Vector2 b, float tolerance = 0.0001f)
        {
            return Vector2.SqrMagnitude(a - b) < tolerance * tolerance;
        }

        public static bool IsApproximately(this Vector3 a, Vector3 b, float tolerance = 0.0001f)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance * tolerance;
        }
    }
}

