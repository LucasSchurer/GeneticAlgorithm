/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EnemyPopulationHandler : MonoBehaviour
{
    [System.Serializable]
    public struct PopulationStatistics
    {
        public int enemiesCount;
        public float damageDealt;
        public float damageTaken;
    }

    [SerializeField]
    private PopulationStatistics _statistics;

    public PopulationStatistics Statistics => _statistics;


    [SerializeField]
    private int _enemiesPerWave = 10;

    [SerializeField]
    private bool _isRespawning = false;

    [SerializeField]
    private Vector2 _arenaSize;

    [SerializeField]
    private Enemy _enemyPrefab;

    [SerializeField]
    private Enemy[] _enemies;

    [SerializeField]
    private float _respawnTimer = 5f;

    [SerializeField]
    private float _populationFitness = 0f;

    [SerializeField]
    private Enemy _fittestEnemy;

    [SerializeField]
    private Transform _enemyContainer;

    public static EnemyPopulationHandler Instance { get; private set; }

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

        _enemies = new Enemy[_enemiesPerWave];
    }

    private void Start()
    {
        SpawnPopulation();
    }

    private void Update()
    {
        if (_enemies.Where(e => !e.isDead).Count() <= 0 && !_isRespawning)
        {
            StartCoroutine(Respawn());
        }
    }

    private void SpawnPopulation()
    {
        for (int i = 0; i < _enemiesPerWave; i++)
        {
            _enemies[i] = Instantiate(_enemyPrefab, _enemyContainer);
            _enemies[i].Initialize();
            _enemies[i].chromosome.RandomizeGenes();
            _enemies[i].UpdateValues();
            _enemies[i].gameObject.SetActive(true);
            _enemies[i].transform.position = GetSpawnPosition();
        }
    }

    private IEnumerator Respawn()
    {
        _isRespawning = true;

        yield return new WaitForSeconds(_respawnTimer);

        GenerateNewPopulation();
        _isRespawning = false;
    }

    private Vector2 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-_arenaSize.x / 2, _arenaSize.x / 2), 0.6f, Random.Range(-_arenaSize.y / 2, _arenaSize.y / 2));
    }

    private void UpdatePopulationFitness()
    {
        _populationFitness = 0;
        _fittestEnemy = _enemies[0];

        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].UpdateFitness();
            _populationFitness += _enemies[i].Fitness;

            if (_fittestEnemy.Fitness < _enemies[i].Fitness)
            {
                _fittestEnemy = _enemies[i];
            }
        }
    }

    /// <summary>
    /// Generate a new set of enemies using a 
    /// genetic algorithm.
    /// </summary>
    private void GenerateNewPopulation()
    {
        Enemy[] newEnemies = new Enemy[_enemiesPerWave];

        UpdatePopulationStatistics();
        UpdatePopulationFitness();

        // Elitist selection
        newEnemies[0] = Instantiate(_enemyPrefab, _enemyContainer);
        newEnemies[1] = Instantiate(_enemyPrefab, _enemyContainer);
        newEnemies[0].Initialize(_fittestEnemy.chromosome);
        newEnemies[1].Initialize(_fittestEnemy.chromosome);

        for (int i = 2; i < _enemies.Length; i+=2)
        {
            // Selection
            int parentA = RouletWheelSelection();
            int parentB = RouletWheelSelection();

            // Crossover
            System.Tuple<Chromosome, Chromosome> offspring = Chromosome.Crossover(_enemies[parentA].chromosome, _enemies[parentB].chromosome);

            // Mutation
            offspring.Item1.Mutate();
            offspring.Item2.Mutate();

            newEnemies[i] = Instantiate(_enemyPrefab, _enemyContainer);
            newEnemies[i + 1] = Instantiate(_enemyPrefab, _enemyContainer);
            
            newEnemies[i].Initialize((EnemyChromosome)offspring.Item1);
            newEnemies[i + 1].Initialize((EnemyChromosome)offspring.Item2);
        }

        // Destroy the old generation and assign the new one to the list of enemies.
        for (int i = 0; i < _enemies.Length; i++)
        {
            Destroy(_enemies[i].gameObject);
            _enemies[i] = newEnemies[i];
            _enemies[i].gameObject.SetActive(true);
            _enemies[i].transform.position = GetSpawnPosition();
        }
    }

    private int RouletWheelSelection()
    {
        float randomFitness = Random.Range(0, _populationFitness);
        float fitnessRange = 0f;

        for (int i = 0; i < _enemies.Length; i++)
        {
            fitnessRange += _enemies[i].Fitness;

            if (fitnessRange > randomFitness)
            {
                return i;
            }
        }

        return 0;
    }

    private void UpdatePopulationStatistics()
    {
        _statistics.damageDealt = 0;
        _statistics.damageTaken = 0;
        _statistics.enemiesCount = _enemies.Length;

        for (int i = 0; i < _enemies.Length; i++)
        {
            _statistics.damageDealt += _enemies[i].Statistics.damageDealt;
            _statistics.damageTaken += _enemies[i].Statistics.damageTaken;
        }
    }
}
*/