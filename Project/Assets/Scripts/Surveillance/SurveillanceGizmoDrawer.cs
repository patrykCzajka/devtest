using UnityEngine;

namespace Scripts.Surveillance
{
    public sealed class SurveillanceGizmoDrawer
    {
        const int Segments = 15;

        public void DrawCone(Vector3 tip, Vector3 forward, float halfAngle, float distance)
        {
            SurveillanceConeMath.Rays(tip, forward, halfAngle, distance, out var leftEnd, out var rightEnd);
            Gizmos.DrawLine(tip, leftEnd);
            Gizmos.DrawLine(tip, rightEnd);

            var baseCenter = tip + forward * distance;
            var ortho = Vector3.Cross(Vector3.up, forward).normalized;
            var radius = Mathf.Tan(Mathf.Deg2Rad * halfAngle) * distance;
            var rot = Quaternion.AngleAxis(360f / Segments, forward);
            var last = baseCenter + ortho * radius;

            for (int i = 0; i <= Segments; i++)
            {
                var next = rot * (last - baseCenter) + baseCenter;
                Gizmos.DrawLine(last, next);
                Gizmos.DrawLine(tip, next);
                last = next;
            }
        }
    }
}