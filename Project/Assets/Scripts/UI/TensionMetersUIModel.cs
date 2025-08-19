using System;
using UnityEngine;

namespace Scripts.UI {
    public abstract class TensionMetersUIModel : ScriptableObject, IUIModel{
        public Action<float> OnProgressChanged;

        public abstract void UpdateProgress(float currentProgress);

        public virtual void Clear() {
            OnProgressChanged = null;
        }
    }
}