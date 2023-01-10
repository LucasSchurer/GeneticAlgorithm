using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        [SerializeField]
        private WaveSettings _waveSettings;

        private void Start()
        {
            PopulationManager populationManager = PopulationManager.Instance;

            if (populationManager)
            {
                populationManager.Initialize(_waveSettings.enemiesPerWave);
                WaveManager.Instance?.Initialize(_waveSettings, populationManager);
                WaveManager.Instance?.StartWave();
            }
        }
    } 
}
