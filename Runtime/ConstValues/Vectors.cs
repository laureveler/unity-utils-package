using UnityEngine;

namespace Utilities.Vectors
{
    public static class V2
    {
        public static Vector2 Up => _up;
        private static readonly Vector2 _up = new Vector2(0, 1);

        public static Vector2 Down => _down;
        private static readonly Vector2 _down = new Vector2(0, -1);

        public static Vector2 Left => _left;
        private static readonly Vector2 _left = new Vector2(-1, 0);

        public static Vector2 Right => _right;
        private static readonly Vector2 _right = new Vector2(1, 0);

        public static Vector2 Zero => _zero;
        private static readonly Vector2 _zero = new Vector2(0, 0);

        public static bool IsPrimitive(this Vector2 vec)
        {
            return vec == Up || vec == Down || vec == Left || vec == Right;
        }
    }

    public static class V3
    {
        public static Vector3 Up => _up;
        private static readonly Vector3 _up = new Vector3(0, 1, 0);

        public static Vector3 Down => _down;
        private static readonly Vector3 _down = new Vector3(0, -1, 0);

        public static Vector3 Left => _left;
        private static readonly Vector3 _left = new Vector3(-1, 0, 0);

        public static Vector3 Right => _right;
        private static readonly Vector3 _right = new Vector3(1, 0, 0);

        public static bool IsPrimitive(this Vector3 vec)
        {
            return vec == Up || vec == Down || vec == Left || vec == Right;
        }
    }
}

