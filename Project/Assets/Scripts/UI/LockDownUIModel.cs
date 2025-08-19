using UnityEngine;

namespace Scripts.UI {
    
    [CreateAssetMenu(fileName = "LockDownUIModel", menuName = "Scripts/UI/LockDownUIModel")]
    public class LockDownUIModel : TensionMetersUIModel {
        
        public override void UpdateProgress(float currentProgress) {
            OnProgressChanged?.Invoke(currentProgress);
        }
    }
}