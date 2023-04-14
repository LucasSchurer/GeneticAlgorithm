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
        
        protected override void SingletonAwake()
        {
            eventController = GetComponent<GameEventController>();
            Cursor.lockState = _cursorLockMode;
        }

        private void Start()
        {
            eventController.TriggerEvent(GameEventType.OnWaveStart, new GameEventContext());
        }

        private void OnApplicationQuit()
        {
            eventController.TriggerEvent(GameEventType.OnApplicationQuit, new GameEventContext());
        }
    } 
}
