using Scripts.Settings;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Surveillance {
    public class SurveillanceIndicatorView : MonoBehaviour, ITick, IRequireGameSettings {
        [SerializeField] SurveillanceCameraRefs _cameraRefs;
        [SerializeField] DetectionAccumulator _detector;
        GameSettingsSO _gameSettings;

        public void Inject(GameSettingsSO settings) {
            _gameSettings = settings;
        }

        public void Tick() {
            if (_cameraRefs == null || _detector == null || _gameSettings == null) return;
            var mat = _detector.CurrentLevel switch {
                DetectionAccumulator.DetectionLevel.OutOfRange => _gameSettings.OutOfRangeBlackMat,
                DetectionAccumulator.DetectionLevel.HiddenButInRange => _gameSettings.OutOfRangeBlackMat,
                DetectionAccumulator.DetectionLevel.InSightInRange => _gameSettings.RedInSightMat,
                DetectionAccumulator.DetectionLevel.StealingInSight => _gameSettings.RedStealingMat,
                _ => _cameraRefs.IndicatorRenderer.sharedMaterial
            };
            _cameraRefs.IndicatorRenderer.sharedMaterial = mat;
        }
    }
}