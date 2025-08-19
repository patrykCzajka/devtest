using UnityEngine;

namespace Scripts.Systems {
    public class Gate : MonoBehaviour {
        [SerializeField] Vector3 targetPosition;
        [SerializeField] Vector3 startPosition;
        float _gateCloseTimeInSeconds;
        Transform _gateTransform;

        void Awake() {
            _gateTransform = transform;
            SetEnabled(false);
        }

        public void SetEnabled(bool enabled) {
            _gateTransform.gameObject.SetActive(enabled);
        }

        public void VisualizeProgress(float progress) {
            _gateTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
        }
    }
}