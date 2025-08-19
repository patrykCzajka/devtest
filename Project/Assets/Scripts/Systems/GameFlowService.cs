using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Systems {
    public enum GameState { Menu, Playing, Lockdown, GameOver, GameFinished }

    public sealed class GameFlowService : MonoBehaviour {
        public event Action<GameState> OnStateChanged;
        [SerializeField] string sceneName = "SampleScene";
        GameState _state = GameState.Menu;
        WaitForSeconds _gameRefreshDelay = new WaitForSeconds(2f);

        public GameState State => _state;

        void Start() {
            SetState(GameState.Playing);
        }

        public void TriggerLockdown() => SetState(GameState.Lockdown);
        public void TriggerGameOver() {
            SetState(GameState.GameOver);
        }
        public void TriggerGameFinished() {
            SetState(GameState.GameFinished);
        }

        void SetState(GameState next) {
            if (_state == next) return;
            _state = next;
            OnStateChanged?.Invoke(_state);

            switch (_state) {
                case GameState.GameOver:
                case GameState.GameFinished:
                    StartCoroutine(ReloadScene());
                    break;
            }
        }

        IEnumerator ReloadScene() {
            yield return _gameRefreshDelay;
            if (!string.IsNullOrEmpty(sceneName))
                SceneManager.LoadScene(sceneName);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}