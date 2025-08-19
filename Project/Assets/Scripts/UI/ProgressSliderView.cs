using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class ProgressSliderView : MonoBehaviour {
        [SerializeField] Slider detectionSlider;
        
        public void UpdateSlider(float progress) {
            detectionSlider.SetValueWithoutNotify(1f-progress);
        }
        public void HideSlider() {
            gameObject.SetActive(false);
        }
        public void ShowSlider() {
            gameObject.SetActive(true);
        }
    }
}