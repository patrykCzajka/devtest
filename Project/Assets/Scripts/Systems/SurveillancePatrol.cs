using Scripts.Settings;
using Scripts.Systems;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Surveillance {
    public class SurveillancePatrol : MonoBehaviour, IInitialize, IRequireGameSettings, ITick, IRequirePlayer, IRequireUI, IRequireGameFlow {
        [SerializeField] SurveillanceCameraRefs[] surveillanceCameraRefs;
        
        GameSettingsSO _gameSettings;
        Player.Player _player;
        float _maxDetectionTime;
        float _currentDetectionTime = 0f;
        float _stealthRetrievalRate;
        bool _lockDownTriggered;
        UIModelsReferences _uiRefs;
        GameFlowService _gameFlowService;

        [ContextMenu("find refs")]
        void FindRefs(){
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Find References");
            surveillanceCameraRefs = FindObjectsOfType<SurveillanceCameraRefs>();
#endif
        }

        public void Tick() {
            if (_lockDownTriggered)
                return;
            int numberOfCamerasDetectingPlayerStealing = 0;
            int numberOfCamerasDetectingPlayer = 0;
            foreach (SurveillanceCameraRefs s in surveillanceCameraRefs) {
                s.Camera.Tick();
                numberOfCamerasDetectingPlayer += s.StealingDetector.CurrentDetectionLevel ==
                                                  DetectionAccumulator.DetectionLevel.InSightInRange
                    ? 1
                    : 0;
                numberOfCamerasDetectingPlayerStealing += s.StealingDetector.CurrentDetectionLevel ==
                                                          DetectionAccumulator.DetectionLevel.StealingInSight
                    ? 1
                    : 0;
            }
            CalculateDetectionTime(numberOfCamerasDetectingPlayer, numberOfCamerasDetectingPlayerStealing);
        }

        void CalculateDetectionTime(int camerasDetectedPlayer, int camerasDetectedPlayerStealing) {
            if(camerasDetectedPlayerStealing > 0)
                _currentDetectionTime += Time.deltaTime * _gameSettings.StealingStatusStealthLoseSpeed;
            else if (camerasDetectedPlayer > 0)
                _currentDetectionTime += Time.deltaTime;
            else if(_gameSettings.StealthRetrievalOn)
                _currentDetectionTime -= Time.deltaTime * _stealthRetrievalRate;

            _currentDetectionTime = Mathf.Clamp(_currentDetectionTime, 0f, _maxDetectionTime);
            _uiRefs.StealthUIModel.UpdateProgress(_currentDetectionTime);
            if(_currentDetectionTime >= _maxDetectionTime)
                _gameFlowService.TriggerLockdown();
        }

        public void Inject(GameSettingsSO settings) {
            _gameSettings = settings;
            _maxDetectionTime = settings.DetectionTime;
            _stealthRetrievalRate = settings.StealthRetrievalRate;

        }
        public void Inject(Player.Player player) {
            _player = player;
        }

        public void Initialize() {
            foreach (SurveillanceCameraRefs s in surveillanceCameraRefs) {
                s.Camera.Initialize(_gameSettings, _player);
            }

            _gameFlowService.OnStateChanged += HandleStateChanged;
        }

        void HandleStateChanged(GameState state) {
            if (state != GameState.Lockdown)
                return;
            _lockDownTriggered = true;
        }
        public void Inject(UIModelsReferences ui) {
            _uiRefs = ui;
        }

        public void Inject(GameFlowService gameFlowService) {
            _gameFlowService = gameFlowService;
        }
    }
}