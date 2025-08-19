using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class PlayerProgressView : MonoBehaviour {
        [SerializeField] Slider progressSlider;

        public void UpdateSlider(float progress) {
            progressSlider.SetValueWithoutNotify(progress);
        }
    }
}