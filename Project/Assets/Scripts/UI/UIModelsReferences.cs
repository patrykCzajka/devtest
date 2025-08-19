using UnityEngine;

namespace Scripts.UI {
    [CreateAssetMenu(fileName = "UI Models", menuName = "Scripts/UI/UI Model References")]
    public class UIModelsReferences : ScriptableObject {
        public StealthUIModel StealthUIModel => stealthUIModel;
        public LockDownUIModel LockDownUIModel => lockDownUIModel;
        public PlayerProgressUIModel PlayerProgressUIModel => playerProgressUIModel;
        public InteractionTipUIModel InteractionTipUIModel => interactionTipUIModel;
        public GameOverUIModel GameOverUIModel => gameOverUIModel;
        public GameFinishedUIModel GameFinishedUIModel => gameFinishedUIModel;

        [SerializeField] StealthUIModel stealthUIModel;
        [SerializeField] LockDownUIModel lockDownUIModel;
        [SerializeField] PlayerProgressUIModel playerProgressUIModel;
        [SerializeField] InteractionTipUIModel interactionTipUIModel;
        [SerializeField] GameOverUIModel gameOverUIModel;
        [SerializeField] GameFinishedUIModel gameFinishedUIModel;

        public void ClearAllCallbacks() {
            stealthUIModel.Clear();
            lockDownUIModel.Clear();
            playerProgressUIModel.Clear();
            interactionTipUIModel.Clear();
            gameOverUIModel.Clear();
            gameFinishedUIModel.Clear();
        }
    }
}