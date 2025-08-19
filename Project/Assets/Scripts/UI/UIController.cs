using System;
using Scripts.Settings;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.UI {
    public class UIController : MonoBehaviour, IRequireGameSettings, IInitialize, IRequireUI, IRequireGameFlow {
        [SerializeField] ProgressSliderView detectionView;
        [SerializeField] ProgressSliderView lockDownProgressView;
        [SerializeField] PlayerProgressView progressView;
        [SerializeField] InteractionTipView interactionView;
        [SerializeField] GameOverView gameOverView;
        [SerializeField] GameOverView gameFinishedView;
        GameSettingsSO _gameSettings;
        UIModelsReferences _uiRefs;
        GameFlowService _gameFlowService;
        bool _initialized;

        public void Inject(GameSettingsSO settings) {
            _gameSettings = settings;
        }

        public void Initialize() {
            _uiRefs.StealthUIModel.Setup(_gameSettings.DetectionTime);
            _uiRefs.StealthUIModel.OnProgressChanged += detectionView.UpdateSlider;
            _uiRefs.StealthUIModel.OnStealthBarHide += detectionView.HideSlider;
            _uiRefs.StealthUIModel.OnStealthBarHide += lockDownProgressView.ShowSlider;
            
            _uiRefs.LockDownUIModel.OnProgressChanged += lockDownProgressView.UpdateSlider;

            _uiRefs.PlayerProgressUIModel.OnProgressChanged += progressView.UpdateSlider;

            _uiRefs.InteractionTipUIModel.OnTipChanged += interactionView.UpdateTip;
            _uiRefs.GameOverUIModel.OnGameOverTriggered += gameOverView.SetPanelActive;
            
            _uiRefs.GameFinishedUIModel.OnGameFinished += gameFinishedView.SetPanelActive;

            detectionView.UpdateSlider(0f);
            progressView.UpdateSlider(0f);
            interactionView.UpdateTip(string.Empty);
            lockDownProgressView.HideSlider();
            gameOverView.HidePanel();
            gameFinishedView.HidePanel();
            _gameFlowService.OnStateChanged += HandleGameStateChanged;
            _initialized = true;
        }

        void HandleGameStateChanged(GameState state) {
            switch (state) {
                case GameState.GameOver:
                    _uiRefs.GameOverUIModel.ShowGameOverPanel();
                    break;
                case GameState.GameFinished:
                    _uiRefs.GameFinishedUIModel.ShowGameFinishedPanel();
                    break;
                case GameState.Menu:
                case GameState.Playing:
                case GameState.Lockdown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        void OnDestroy() {
            if (!_initialized)
                return;
            _uiRefs.ClearAllCallbacks();
            _gameFlowService.OnStateChanged -= HandleGameStateChanged;
        }

        public void Inject(UIModelsReferences uiModelsReferences) {
            _uiRefs = uiModelsReferences;
        }

        public void Inject(GameFlowService gameFlowService) {
            _gameFlowService = gameFlowService;
        }
    }
}