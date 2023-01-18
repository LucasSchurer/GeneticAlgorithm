using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class WaveManager : MonoBehaviour, IEventListener
    {
        public static WaveManager Instance { get; private set; }

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

        [Header("References")]
        [SerializeField]
        private Transform _spawnPosition;
        private PopulationManager _populationManager;

        [Header("Settings")]
        private WaveSettings _waveSettings;
        private float _timeRemaining = 0;
        private bool _isSpawningEnemies = false;
        private bool _isWaveActive = false;

        public float TimeRemaining => _timeRemaining;
        public bool IsWaveActive => _isWaveActive;

        public void Initialize(WaveSettings waveSettings, PopulationManager populationManager)
        {
            _waveSettings = waveSettings;
            _populationManager = populationManager;
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 position = _spawnPosition.position;
            position.x += Random.Range(-10f, 10f);
            position.z += Random.Range(-10f, 10f);

            return position;
        }

        public void StartWave()
        {
            _timeRemaining = _waveSettings.waveDuration;
            _isSpawningEnemies = true;
            _isWaveActive = true;

            StartCoroutine(SpawnCoroutine());
        }

        private void Update()
        {
            if (_isWaveActive)
            {
                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining <= 0)
                {
                    _isWaveActive = false;
                    StopAllCoroutines();
                    GameManager.Instance.eventController.TriggerEvent(GameEventType.OnWaveEnd, new GameEventContext());
                }
            }
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                GA.CreatureController creature = _populationManager.RequestCreature();

                if (creature != null)
                {
                    creature.transform.position = GetSpawnPosition();
                    creature.gameObject.SetActive(true);

                    yield return new WaitForSeconds(Random.Range(_waveSettings.minSpawnInterval, _waveSettings.maxSpawnInterval));
                }
                else
                {
                    break;
                }
            }

            _isSpawningEnemies = false;
            yield return null;
        }
        private void StartWave(ref GameEventContext ctx)
        {
            StartWave();   
        }
        private void RespawnWave(ref GameEventContext ctx)
        {
            StartCoroutine(RespawnWaveCoroutine());
        }

        private IEnumerator RespawnWaveCoroutine()
        {
            yield return new WaitForSeconds(_waveSettings.waveRespawnTime);

            GameManager.Instance?.eventController.TriggerEvent(GameEventType.OnWaveStart, new GameEventContext());
        }

        public void StartListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.eventController.AddListener(GameEventType.OnWaveEnd, RespawnWave);
                gameManager.eventController.AddListener(GameEventType.OnWaveStart, StartWave);
            }
        }

        public void StopListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.eventController.RemoveListener(GameEventType.OnWaveEnd, RespawnWave);
                gameManager.eventController.RemoveListener(GameEventType.OnWaveStart, StartWave);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }
    }

}