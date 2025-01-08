using UnityEngine;

namespace Utilities.Rigidbody
{
    public static class RigidbodyExtensions
    {
        /// <summary>
        /// Rotate towards a target direction. The rotation will stop when the target direction is reached.
        /// To be used in FixedUpdate.
        /// </summary>
        public static void RotateTowards(this Rigidbody2D rb, Vector2 targetDir, float rotateSpeed)
        {
            if (targetDir != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
                float angle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotateSpeed * Time.fixedDeltaTime);

                rb.MoveRotation(angle);
            }
        }

        /// <summary>
        /// Continually rotate in a direction. To be used in FixedUpdate.
        /// </summary>
        public static void ApplyRotation(this Rigidbody2D rb, float rotationDirection, float speed)
        {
            float rotationAmount = rotationDirection * speed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotationAmount);
        }
    }
}