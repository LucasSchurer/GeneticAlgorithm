using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EnemyPopulationHandler : MonoBehaviour
{
    [SerializeField]
    private int _enemiesPerWave = 10;

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

    private void Awake()
    {
        _enemies = new Enemy[_enemiesPerWave];
    }

    private void Start()
    {
        SpawnEnemies();
        /*Selection();*/
        /*StartCoroutine(Respawn());*/
    }

    private void Update()
    {
        if (_enemies.Where(e => e.gameObject.activeSelf).Count() <= 0)
        {
            Selection();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < _enemiesPerWave; i++)
        {
            _enemies[i] = Instantiate(_enemyPrefab, transform);
            _enemies[i].Initialize();
            _enemies[i].chromosome.RandomizeGenes();
            _enemies[i].UpdateValues();
            _enemies[i].gameObject.SetActive(true);
            _enemies[i].transform.position = GetSpawnPosition();
        }
    }

    private void DestroyEnemies()
    {
        for (int i = 0; i < _enemiesPerWave; i++)
        {
            Destroy(_enemies[i].gameObject);
            _enemies[i] = null;
        }
    }

    private Vector2 GetSpawnPosition()
    {
        return new Vector2(Random.Range(-_arenaSize.x / 2, _arenaSize.x / 2), Random.Range(-_arenaSize.y / 2, _arenaSize.y / 2));
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTimer);

        Selection();
        /*DestroyEnemies();
        SpawnEnemies();*/
        StartCoroutine(Respawn());
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

    private void Selection()
    {
        Enemy[] newEnemies = new Enemy[_enemiesPerWave];

        UpdatePopulationFitness();

        newEnemies[0] = Instantiate(_enemyPrefab, transform);
        newEnemies[1] = Instantiate(_enemyPrefab, transform);
        newEnemies[0].Initialize(_fittestEnemy.chromosome);
        newEnemies[1].Initialize(_fittestEnemy.chromosome);

        for (int i = 2; i < _enemies.Length; i++)
        {
            // Selection
            int parentA = GetParentIndex();
            int parentB = GetParentIndex();

            // Crossover
            System.Tuple<Chromosome, Chromosome> offspring = Chromosome.Crossover(_enemies[GetParentIndex()].chromosome, _enemies[GetParentIndex()].chromosome);

            // Mutation
            offspring.Item1.Mutate();
            offspring.Item2.Mutate();

            newEnemies[i] = Instantiate(_enemyPrefab, transform);
            newEnemies[i + 1] = Instantiate(_enemyPrefab, transform);
            
            newEnemies[i].Initialize((EnemyChromosome)offspring.Item1);
            newEnemies[i + 1].Initialize((EnemyChromosome)offspring.Item2);
            i++;
        }

        for (int i = 0; i < _enemies.Length; i++)
        {
            Destroy(_enemies[i].gameObject);
            _enemies[i] = newEnemies[i];
            _enemies[i].gameObject.SetActive(true);
            _enemies[i].transform.position = GetSpawnPosition();
        }

        UpdatePopulationFitness();
    }

    private int GetParentIndex()
    {
        float randomFitness = StaticRandom.RandomFloat(0, _populationFitness);

        int max = _enemiesPerWave - 1;
        int min = 0;
        float fitnessRange = 0f;

        for (int i = _enemiesPerWave - 1; i >= 0; i--)
        {
            fitnessRange += _enemies[i].Fitness;

            if (fitnessRange > randomFitness)
            {
                return i;
            }
        }

        return 0;
    }
}
