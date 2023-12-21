using Cinemachine;
using Game.Events;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector]
        private GameEventController _eventController;
        [SerializeField]
        private CursorLockMode _cursorLockMode = CursorLockMode.None;
        [SerializeField]
        private string _playerTag = "Player";
        private Transform _player;
        private bool _isPaused = false;

        public bool IsPaused => _isPaused;

        private bool _gameOver = false;

        [SerializeField]
        private float _timeScale = 1f;

        public GameEventController GetEventController()
        {
            if (_eventController == null)
            {
                _eventController = GetComponent<GameEventController>();
            }

            return _eventController;
        }

        [HideInInspector]
        public Transform Player => _player == null ? GameObject.FindGameObjectWithTag(_playerTag).transform : _player;
        
        protected override void SingletonAwake()
        {
            Cursor.lockState = _cursorLockMode;
            Time.timeScale = _timeScale;
        }

        private void Start()
        {
            _eventController.TriggerEvent(GameEventType.OnWaveStart, new GameEventContext());

            QualitySettings.vSyncCount = GameSettings.Instance.VSyncCount;
        }

        private void OnApplicationQuit()
        {
            _eventController.TriggerEvent(GameEventType.OnApplicationQuit, new GameEventContext());
        }

        public void PauseGame(bool triggerEvent = true)
        {
            if (!_isPaused && !_gameOver)
            {
                _isPaused = true;

                Time.timeScale = 0f;

                CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

                if (brain != null)
                {
                    brain.enabled = false;
                }

                _cursorLockMode = CursorLockMode.Confined;
                Cursor.lockState = _cursorLockMode;
                Cursor.visible = true;

                if (triggerEvent)
                {
                    _eventController.TriggerEvent(GameEventType.OnPause, new GameEventContext() { });
                }
            }
        }

        public void ResumeGame(bool triggerEvent = true)
        {
            if (_isPaused && !_gameOver)
            {
                _isPaused = false;

                Time.timeScale = 1f;

                CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

                if (brain != null)
                {
                    brain.enabled = true;
                }

                _cursorLockMode = CursorLockMode.Locked;
                Cursor.lockState = _cursorLockMode;
                Cursor.visible = false;

                if (triggerEvent)
                {
                    _eventController.TriggerEvent(GameEventType.OnResume, new GameEventContext() { });
                }
            }
        }

        public void GameOver()
        {
            PauseGame(false);

            _gameOver = true;

            _eventController.TriggerEvent(GameEventType.OnGameOver, new GameEventContext() { });
        }

        [ContextMenu("Generate Graph")]
        private void SetTimeScale()
        {
            Time.timeScale = _timeScale;
        }
    } 
}
