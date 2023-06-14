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

        public bool IsPaused => _isPaused;
        
        protected override void SingletonAwake()
        {
            Cursor.lockState = _cursorLockMode;
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

        public void PauseGame()
        {
            if (!_isPaused)
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
            }
        }

        public void ResumeGame()
        {
            if (_isPaused)
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
            }
        }
    } 
}
