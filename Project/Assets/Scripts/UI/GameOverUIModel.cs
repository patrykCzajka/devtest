using System;
using UnityEngine;

namespace Scripts.UI {
    [CreateAssetMenu(fileName = "GameOverUIModel", menuName = "Scripts/UI/GameOverUIModel")]
    public class GameOverUIModel : ScriptableObject, IUIModel {

        public event Action OnGameOverTriggered;
        
        public void ShowGameOverPanel() {
            OnGameOverTriggered?.Invoke();
        }

        public void Clear() {
            OnGameOverTriggered = null;
        }
    }
}