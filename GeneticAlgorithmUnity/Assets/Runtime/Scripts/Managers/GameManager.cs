using Game.Events;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector]
        public GameEventController eventController;
        
        protected override void SingletonAwake()
        {
            eventController = GetComponent<GameEventController>();
        }

        private void Start()
        {
            eventController.TriggerEvent(GameEventType.OnWaveStart, new GameEventContext());
        }
    } 
}
