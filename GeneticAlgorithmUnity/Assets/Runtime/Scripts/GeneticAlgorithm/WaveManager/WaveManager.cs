using Game.Events;
using System.Collections;
using UnityEngine;
using Game.GA;
using Game.InteractableObjects;

namespace Game.Managers
{
    public class WaveManager : Singleton<WaveManager>, IEventListener
    {
        [Header("References")]
        [SerializeField]
        private float _spawnRadius;
        [SerializeField]
        private float _minimumDistanceToSpawn;
        [SerializeField]
        private int _maximumSpawnRetry = 15;
        [SerializeField]
        private InteractableTraitObject _interactableTraitObjectPrefab;

        [SerializeField]
        private Transform _team1SpawnPosition;
        [SerializeField]
        private Transform _team2SpawnPosition;

        private PopulationController _populationController;

        [SerializeField]
        private EntitySpawner _team1Spawner;
        [SerializeField]
        private EntitySpawner _team2Spawner;

        [Header("Settings")]
        public WaveSettings waveSettings;
        private float _timeRemaining = 0;
        private bool _isSpawningEnemies = false;
        private bool _isWaveActive = false;

        public float TimeRemaining => _timeRemaining;
        public bool IsWaveActive => _isWaveActive;

        protected override void SingletonAwake()
        {
        }

        private Vector3 GetSpawnPosition(Vector3 basePosition)
        {
            Vector3 position = Random.insideUnitCircle * Random.Range(_minimumDistanceToSpawn, _spawnRadius);

            position.z = position.y;
            position.y = 0f;

            position.y = 5f;

            position += basePosition;

            if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, Constants.GroundLayer))
            {
                position = hit.point;
                position.y += 1.5f;
            }

            return position;
        }

        public void StartWave()
        {
            _timeRemaining = waveSettings.waveDuration;
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
                    GameManager.Instance.GetEventController().TriggerEvent(GameEventType.OnWaveEnd, new GameEventContext());
                }
            }
        }

        private IEnumerator SpawnCoroutine()
        {
            Transform[] team1Creatures = _team1Spawner.GetEntities();

            for (int i = 0; i < team1Creatures.Length; i++)
            {
                team1Creatures[i].transform.position = GetSpawnPosition(_team1SpawnPosition.position);
            }

            Transform[] team2Creatures = _team2Spawner.GetEntities();

            for (int i = 0; i < team2Creatures.Length; i++)
            {
                team2Creatures[i].transform.position = GetSpawnPosition(_team2SpawnPosition.position);
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
            yield return new WaitForSeconds(waveSettings.waveRespawnTime);

            GameManager.Instance?.GetEventController().TriggerEvent(GameEventType.OnWaveStart, new GameEventContext());
        }

        public void StartListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.GetEventController().AddListener(GameEventType.OnWaveEnd, RespawnWave);
                gameManager.GetEventController().AddListener(GameEventType.OnWaveStart, StartWave, EventExecutionOrder.After);
            }
        }

        public void StopListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveEnd, RespawnWave);
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveStart, StartWave, EventExecutionOrder.After);
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

        private void OnDrawGizmosSelected()
        {
            Color color = Gizmos.color;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_team1SpawnPosition.position, _spawnRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(_team1SpawnPosition.position, _minimumDistanceToSpawn);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_team2SpawnPosition.position, _spawnRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(_team2SpawnPosition.position, _minimumDistanceToSpawn);

            Gizmos.color = color;
        }
    }
}