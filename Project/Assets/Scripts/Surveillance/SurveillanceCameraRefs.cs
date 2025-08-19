using UnityEngine;

namespace Scripts.Surveillance {
    public class SurveillanceCameraRefs : MonoBehaviour {
        [SerializeField] SurveillanceCameraRotationBehaviour cameraRotation;
        [SerializeField] SurveillanceCamera camera;
        [SerializeField] DetectionAccumulator stealingDetector;
        [SerializeField] Transform cameraTransform;
        [SerializeField] MeshRenderer indicatorRenderer;
        public SurveillanceCameraRotationBehaviour CameraRotation => cameraRotation;
        public SurveillanceCamera Camera => camera;
        public DetectionAccumulator StealingDetector => stealingDetector;
        public Transform CameraTransform => cameraTransform;
        public MeshRenderer IndicatorRenderer => indicatorRenderer;
        Vector3? _cameraStartForward;

        void Awake() {
            _cameraStartForward = CameraTransform.forward;
        }
        
        public Vector3 GetCameraForwardNormalized() {
            return CameraTransform.forward;
        }
        public Vector3 GetCameraStartingForward() {
            if (_cameraStartForward == null) {
                _cameraStartForward = CameraTransform.forward;
            }
            return _cameraStartForward.Value;
        }
        public Vector3 GetCameraPosition() {
            return CameraTransform.position;
        }
        public Vector3 GetCameraUpDirection() {
            return CameraTransform.up;
        }
    }
}
