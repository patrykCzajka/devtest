using System.Collections;
using Scripts.Settings;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Systems {
    public class LockdownInitiator : MonoBehaviour, IRequireGameSettings, IRequireUI, IInitialize, IRequireGameFlow {
        [SerializeField] Gate[] gates;
        bool _lockdownTriggered;
        float _gateCloseTimeInSeconds;
        UIModelsReferences _uiRefs;
        bool _initialized;
        GameFlowService _gameFlowService;


        public void Initialize() {
            _initialized = true;
            _gameFlowService.OnStateChanged += HandleStateChanged;
        }

        void HandleStateChanged(GameState state) {
            if (state != GameState.Lockdown)
                return;
            InitiateLockdown();
        }

        void OnDestroy() {
            if (_initialized)
                _gameFlowService.OnStateChanged -= HandleStateChanged;
        }

        public void Inject(GameSettingsSO settings) {
            _gateCloseTimeInSeconds = settings.LockdownGateCloseTimeInSeconds;
        }

        public void Inject(UIModelsReferences uiModelReferences) {
            _uiRefs = uiModelReferences;
        }

        void InitiateLockdown() {
            if (_lockdownTriggered)
                return;
            _uiRefs.StealthUIModel.SetEnabled(false);
            _lockdownTriggered = true;
            foreach (var gate in gates) {
                gate.SetEnabled(true);
            }

            StartCoroutine(CloseGatesCor());
            return;

            IEnumerator CloseGatesCor() {
                var progress = 0f;
                while (progress < 1f) {
                    _uiRefs.LockDownUIModel.UpdateProgress(progress);
                    foreach (var gate in gates) {
                        progress += Time.deltaTime / _gateCloseTimeInSeconds;
                        progress = Mathf.Clamp01(progress);
                        var easedTime = Mathf.Pow(progress, 3f);
                        ; //speeding up at the end
                        gate.VisualizeProgress(easedTime);
                        yield return null;
                    }
                }

                _gameFlowService.TriggerGameOver();
            }

        }

        public void Inject(GameFlowService gameFlowService) {
            _gameFlowService = gameFlowService;
        }
    }
}