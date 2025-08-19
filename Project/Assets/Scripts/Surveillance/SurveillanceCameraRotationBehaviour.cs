using UnityEngine;

namespace Scripts.Surveillance {
    public class SurveillanceCameraRotationBehaviour : MonoBehaviour {
        [SerializeField] SurveillanceCameraRefs cameraRefs;
        [SerializeField] bool localAngleOverride;
        [SerializeField] float angleOverrideValue;

        public float RotationAngleLimit => _rotationAngleLimit;
        public float AngleOverrideValue => angleOverrideValue;
        public bool LocalAngleOverride => localAngleOverride;
        
        float _startAngle;
        float _rotationAngleLimit;
        float _speed;
        float _edgePauseDuration;
        float _timeRemainingInEdgePause = 0f; 
        bool _isCameraTakingABreather = false;
        bool _isRotatingTowardPositiveYaw = true; 
        float _currentYawAngle; 
        float _desiredYawAngle = 0f;
        bool _initialized;
        Vector3 _startCameraEulerAngles;

        public void Setup(float angle, float speed, float edgePauseDuration) {
            _currentYawAngle = cameraRefs.CameraTransform.localEulerAngles.y;
            _startAngle = _currentYawAngle;
            _rotationAngleLimit = localAngleOverride ? angleOverrideValue : angle;
            _speed = speed;
            _edgePauseDuration = edgePauseDuration;
            _desiredYawAngle = _startAngle + _rotationAngleLimit;
            _startCameraEulerAngles = cameraRefs.CameraTransform.localRotation.eulerAngles; 
            _initialized = true;
        }

        public void Tick() {
            if (!_initialized)
                return;
            if (_isCameraTakingABreather)
            {
                _timeRemainingInEdgePause -= Time.deltaTime;
                if (!(_timeRemainingInEdgePause <= 0f)) 
                    return;
                _isCameraTakingABreather = false;
                _isRotatingTowardPositiveYaw = !_isRotatingTowardPositiveYaw;
                _desiredYawAngle = _isRotatingTowardPositiveYaw 
                    ? _startAngle + _rotationAngleLimit 
                    : _startAngle - _rotationAngleLimit;
                return;
            }

            float rotationStepThisFrame = _speed * Time.deltaTime;
            _currentYawAngle = Mathf.MoveTowards(
                _currentYawAngle,
                _desiredYawAngle,
                rotationStepThisFrame
            );

            cameraRefs.CameraTransform.localRotation = Quaternion.Euler(_startCameraEulerAngles.x, _currentYawAngle, _startCameraEulerAngles.z);

            if (!Mathf.Approximately(_currentYawAngle, _desiredYawAngle)) 
                return;
            _isCameraTakingABreather = true;
            _timeRemainingInEdgePause = _edgePauseDuration;
        }
    }
}