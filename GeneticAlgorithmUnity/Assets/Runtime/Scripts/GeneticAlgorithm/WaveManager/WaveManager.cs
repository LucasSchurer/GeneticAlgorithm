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

        private PopulationController _populationController;

        [Header("Settings")]
        public WaveSettings waveSettings;
        private float _timeRemaining = 0;
        private bool _isSpawningEnemies = false;
        private bool _isWaveActive = false;

        private Transform _player;

        public float TimeRemaining => _timeRemaining;
        public bool IsWaveActive => _isWaveActive;

        protected override void SingletonAwake()
        {
            _populationController = FindObjectOfType<PopulationController>();
            _player = GameManager.Instance.Player;
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 position = Vector3.zero;
            Vector3 playerPosition = _player.position;
            playerPosition.y = 0f;

            for (int i = 0; i < _maximumSpawnRetry; i++)
            {
                position = Random.insideUnitCircle * Random.Range(_minimumDistanceToSpawn, _spawnRadius);

                position.z = position.y;
                position.y = 0f;

                if (Vector3.Distance(position, playerPosition) > _minimumDistanceToSpawn)
                {                    
                    position.y = 5f;

                    if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, Constants.GroundLayer))
                    {
                        position = hit.point;
                        position.y += 1.5f;
                    }

                    break;
                }
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
            while (true)
            {
                CreatureController creature = _populationController.RequestCreature(GetSpawnPosition());

                if (creature != null)
                {
                    yield return new WaitForSeconds(Random.Range(waveSettings.minSpawnInterval, waveSettings.maxSpawnInterval));
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
            SpawnTraitObjects();

            StartCoroutine(RespawnWaveCoroutine());
        }

        private void SpawnTraitObjects()
        {
            Vector3 position = _player.position;

            for (int i = 0; i < waveSettings.traitsGivenOnWaveEnd; i++)
            {
                position.x = Random.Range(-3, 3) * 2f;
                position.z = Random.Range(-3, 3) * 2f;

                Instantiate(_interactableTraitObjectPrefab, position, Quaternion.identity);
            }
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
                gameManager.GetEventController().AddListener(GameEventType.OnWaveStart, StartWave);
            }
        }

        public void StopListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveEnd, RespawnWave);
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveStart, StartWave);
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
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _minimumDistanceToSpawn);

            Gizmos.color = color;
        }
    }
}