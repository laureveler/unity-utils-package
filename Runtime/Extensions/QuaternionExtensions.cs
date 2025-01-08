using UnityEngine;

namespace Utilities.Quaternions
{
    public static class QuaternionExtensions
    {
        public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange)
        {
            return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
        }
    }
}

