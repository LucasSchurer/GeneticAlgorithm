using Game.Events;
using UnityEngine;
using Game.GA;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector]
        public GameEventController eventController;
        public LayerMask playerLayerMask;
        public LayerMask avoidanceLayerMask;
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
