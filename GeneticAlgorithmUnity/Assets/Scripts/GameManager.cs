using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            PopulationManager populationManager = PopulationManager.Instance;

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
