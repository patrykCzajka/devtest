using System;
using UnityEngine;

namespace Scripts.UI {
    [CreateAssetMenu(fileName = "StealthUIModel", menuName = "Scripts/UI/StealthUIModel")]
    public class StealthUIModel : TensionMetersUIModel {
        public event Action OnStealthBarHide;
        float _maxDetectionTime;

        public void Setup(float detectionTime) {
            _maxDetectionTime = detectionTime;
        }

        public void SetEnabled(bool enabled) {
            if (!enabled) {
                OnStealthBarHide?.Invoke();
            }
        }

        public override void UpdateProgress(float currentProgress) {
            currentProgress = Mathf.Clamp(currentProgress, 0f, _maxDetectionTime);
            OnProgressChanged?.Invoke(currentProgress / _maxDetectionTime);
        }

        public override void Clear() {
            base.Clear();
            OnStealthBarHide = null;
        }
    }
}