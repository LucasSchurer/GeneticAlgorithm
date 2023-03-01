using Game.Events;
using UnityEngine;
using Game.GA;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [HideInInspector]
        public GameEventController eventController;

        [SerializeField]
        private WaveSettings _waveSettings;

        private void Start()
        {
            PopulationController populationManager = PopulationController.Instance;

            if (populationManager)
            {
                populationManager.Initialize(_waveSettings.enemiesPerWave);
                WaveManager.Instance?.Initialize(_waveSettings, populationManager);
            }

            eventController.TriggerEvent(GameEventType.OnWaveStart, new GameEventContext());
        }

        protected override void SingletonAwake()
        {
            eventController = GetComponent<GameEventController>();
        }
    } 
}
