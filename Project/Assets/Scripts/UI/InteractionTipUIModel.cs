using System;
using UnityEngine;

namespace Scripts.UI {
    [CreateAssetMenu(fileName = "InteractionTipUIModel", menuName = "Scripts/UI/InteractionTipUIModel")]
    public class InteractionTipUIModel : ScriptableObject, IInteractionTipUI, IUIModel {

        public event Action<string> OnTipChanged;

        public void ShowTip(string text) {
            OnTipChanged?.Invoke(text);
        }
        public void HideTip() {
            OnTipChanged?.Invoke(string.Empty);
        }

        public void Clear() {
            OnTipChanged = null;
        }
    }
}