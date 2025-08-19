using Scripts.Player;
using Scripts.Settings;
using Scripts.Surveillance;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
{
    [SerializeField] SurveillanceCameraRefs cameraRefs;
    
    public void Initialize(GameSettingsSO settings, Player player) {
        cameraRefs.CameraRotation.Setup(settings.CameraRotationAngle, settings.CameraRotationSpeed, settings.CameraEdgeRotationPauseDuration);
        cameraRefs.StealingDetector.Setup(cameraRefs, settings);
        cameraRefs.StealingDetector.Setup(player);
    }

    public void Tick() {
        cameraRefs.CameraRotation.Tick();
        cameraRefs.StealingDetector.Tick();
    }
}
