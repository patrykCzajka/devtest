using TMPro;
using UnityEngine;

namespace Scripts.UI {
    public class InteractionTipView : MonoBehaviour {
        [SerializeField] TextMeshProUGUI txtField;

        public void UpdateTip(string tipInfo) {
            txtField.text = tipInfo;
        }
    }
}