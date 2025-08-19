using Scripts.Settings;
using Scripts.Surveillance;
using UnityEngine;

public class SurveillanceGizmos : MonoBehaviour {
    [SerializeField] SurveillanceCameraRefs cameraRefs;
    [SerializeField] GameSettingsSO gameSettings;
    const int CONE_SEGMENTS = 15;
    void OnDrawGizmos() {
        if (!Application.isPlaying)
            Gizmos.color = Color.gray;
    
        
        DrawLimitLines(cameraRefs.CameraTransform.position, cameraRefs.CameraRotation.RotationAngleLimit);
        DetermineConeColor();
        DrawConeGizmo(cameraRefs.GetCameraPosition(), cameraRefs.GetCameraForwardNormalized(), gameSettings.RecognitionAngle/2f, gameSettings.RecognitionDistance);
    }

    void DetermineConeColor() {
        if (!Application.isPlaying) {
            Gizmos.color = Color.gray;
            return;
        }
        switch (cameraRefs.StealingDetector.CurrentDetectionLevel) {
            case DetectionAccumulator.DetectionLevel.StealingInSight:
                Gizmos.color = Color.red;
                break;
            case DetectionAccumulator.DetectionLevel.InSightInRange:
                Gizmos.color = Color.yellow;
                break;
            case DetectionAccumulator.DetectionLevel.HiddenButInRange:
                Gizmos.color = Color.green;
                break;
            case DetectionAccumulator.DetectionLevel.OutOfRange:
                Gizmos.color = Color.white;
                break;
        }
    }

    void DrawLimitLines(Vector3 position, float angle) {
        Gizmos.color = Color.blue;
        
        if (!Application.isPlaying) {
            angle = cameraRefs.CameraRotation.LocalAngleOverride
                ? cameraRefs.CameraRotation.AngleOverrideValue
                : gameSettings.CameraRotationAngle;
        }
        Quaternion leftRotation = Quaternion.AngleAxis(-angle, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(angle, Vector3.up);

        Vector3 leftDirection = leftRotation * cameraRefs.GetCameraStartingForward();
        Vector3 rightDirection = rightRotation * cameraRefs.GetCameraStartingForward();

        Gizmos.DrawLine(position, position + leftDirection * 2);
        Gizmos.DrawLine(position, position + rightDirection * 2);
    }
    void DrawConeGizmo(Vector3 origin, Vector3 direction, float angle, float height)
    {

        direction = direction.normalized;
        Vector3 tip = origin;
        
        Vector3 baseCenter = tip + direction * height;

        float radius = height * Mathf.Tan(angle * Mathf.Deg2Rad);

        Vector3 orthoVector = Vector3.Cross(direction, Vector3.up);
        if (orthoVector == Vector3.zero)
            orthoVector = Vector3.Cross(direction, Vector3.right);
        orthoVector.Normalize();

        Quaternion rotation = Quaternion.AngleAxis(360f / CONE_SEGMENTS, direction);
        Vector3 lastPoint = baseCenter + orthoVector * radius;

        for (int i = 0; i <= CONE_SEGMENTS; i++) {
            Vector3 nextPoint = rotation * (lastPoint - baseCenter) + baseCenter;

            Gizmos.DrawLine(lastPoint, nextPoint);
            Gizmos.DrawLine(tip, nextPoint);

            lastPoint = nextPoint;
        }
        //Gizmos.DrawWireSphere(baseCenter, radius);
    }
}

