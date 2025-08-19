using System;
using UnityEngine;

namespace Scripts.UI {
    [CreateAssetMenu(fileName = "PlayerProgressUIModel", menuName = "Scripts/UI/PlayerProgressUIModel")]
    public class PlayerProgressUIModel : ScriptableObject, IUIModel {

        public event Action<float> OnProgressChanged;

        public void VisualizeStealProgress(int numberOfStolenObjects, int numberOfRequiredObjectsToSteal) {
            float progress = (float)numberOfStolenObjects / numberOfRequiredObjectsToSteal;
            OnProgressChanged?.Invoke(progress);
        }

        public void Clear() {
            OnProgressChanged = null;
        }
    }
}