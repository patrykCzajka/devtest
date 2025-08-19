using System;
using Scripts.Settings;
using UnityEngine;

namespace Scripts.Surveillance {
    public class DetectionAccumulator : MonoBehaviour {
        SurveillanceCameraRefs _cameraRefs;
        GameSettingsSO _gameSettings;
        Player.Player _player;
        
        public enum DetectionLevel {
            OutOfRange,
            HiddenButInRange,
            InSightInRange,
            StealingInSight
        }

        public DetectionLevel CurrentDetectionLevel => _currentDetectionLevel;
        DetectionLevel _currentDetectionLevel;
        public DetectionLevel CurrentLevel => _currentDetectionLevel;

        public void Tick() {
            DetermineAndSetDetectionLevel(GetPlayerDetectionCriteria());
        }

        (bool playerAtCameraDistance, bool playerInCameraAngleScope) GetPlayerDetectionCriteria() {
            var cameraToPlayerDir = _player.PlayerTransform.position - _cameraRefs.CameraTransform.position;
            var cameraToPlayerDirNormalized = cameraToPlayerDir.normalized;

            var cameraForward = _cameraRefs.GetCameraForwardNormalized();

            bool playerAtCameraDistance = cameraToPlayerDir.magnitude < _gameSettings.RecognitionDistance;

            bool playerInCameraAngleScope = Vector3.Dot(cameraToPlayerDirNormalized, cameraForward) >=
                                            ConvertAngleToDotProductValue(_gameSettings.RecognitionAngle);
            return (playerAtCameraDistance, playerInCameraAngleScope);
        }

        void DetermineAndSetDetectionLevel(
            (bool playerAtCameraDistance, bool playerInCameraAngleScope) calculatePlayerDetectionCriteria) {
            if (calculatePlayerDetectionCriteria.playerAtCameraDistance && calculatePlayerDetectionCriteria.playerInCameraAngleScope) {
                _currentDetectionLevel = PlayerIsInPlainSight() ? _player.IsCurrentlyHoldingAnObject() ? DetectionLevel.StealingInSight : DetectionLevel.InSightInRange : DetectionLevel.HiddenButInRange;
                return;
            }

            _currentDetectionLevel = DetectionLevel.OutOfRange;

            bool PlayerIsInPlainSight() {
                return !UnityEngine.Physics.Linecast(_cameraRefs.CameraTransform.position, _player.CameraTransform.position, _gameSettings.VisibleObjectsLayers); //TODO: move all physics related calculations to the CustomPhysics or CollisionDetection script.
            }
        }

        public void Setup(SurveillanceCameraRefs cameraRefs, GameSettingsSO settings) {
            _cameraRefs = cameraRefs;
            _gameSettings = settings;
        }
        public void Setup(Player.Player player) {
            _player = player;
        }
        static float ConvertAngleToDotProductValue(float angleDegrees)
        {
            return MathF.Cos(angleDegrees * 0.5f * MathF.PI / 180f);
        }
    }
}
