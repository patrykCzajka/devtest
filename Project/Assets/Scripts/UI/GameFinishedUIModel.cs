using System;
using UnityEngine;

namespace Scripts.UI {
    [CreateAssetMenu(fileName = "GameFinishedUIModel", menuName = "Scripts/UI/GameFinishedUIModel")]
    public class GameFinishedUIModel : ScriptableObject, IUIModel {

        public event Action OnGameFinished;
        
        public void ShowGameFinishedPanel() {
            OnGameFinished?.Invoke();
        }

        public void Clear() {
            OnGameFinished = null;
        }
    }
}