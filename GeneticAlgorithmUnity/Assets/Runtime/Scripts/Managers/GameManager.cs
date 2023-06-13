using Game.Events;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector]
        public GameEventController eventController;
        [SerializeField]
        private CursorLockMode _cursorLockMode = CursorLockMode.None;
        [SerializeField]
        private string _playerTag = "Player";
        private Transform _player;
        [HideInInspector]
        public Transform Player => _player == null ? GameObject.FindGameObjectWithTag(_playerTag).transform : _player; 
        
        protected override void SingletonAwake()
        {
            eventController = GetComponent<GameEventController>();
            Cursor.lockState = _cursorLockMode;
        }

        private void Start()
        {
            eventController.TriggerEvent(GameEventType.OnWaveStart, new GameEventContext());

            QualitySettings.vSyncCount = GameSettings.Instance.VSyncCount;
        }

        private void OnApplicationQuit()
        {
            eventController.TriggerEvent(GameEventType.OnApplicationQuit, new GameEventContext());
        }
    } 
}
