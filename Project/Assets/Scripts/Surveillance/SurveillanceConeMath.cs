using UnityEngine;

namespace Scripts.Surveillance
{
    public static class SurveillanceConeMath
    {
        public static void Directions(float halfAngle, out Quaternion left, out Quaternion right)
        {
            left = Quaternion.AngleAxis(-halfAngle, Vector3.up);
            right = Quaternion.AngleAxis(halfAngle, Vector3.up);
        }

        public static void Rays(Vector3 origin, Vector3 forward, float halfAngle, float distance, out Vector3 leftEnd, out Vector3 rightEnd)
        {
            Directions(halfAngle, out var leftRot, out var rightRot);
            leftEnd = origin + (leftRot * forward) * distance;
            rightEnd = origin + (rightRot * forward) * distance;
        }
    }
}