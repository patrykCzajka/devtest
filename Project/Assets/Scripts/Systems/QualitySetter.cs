using UnityEngine;

namespace Scripts.Systems {
    public class QualitySetter : MonoBehaviour {
        void Awake() {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }
    }
}