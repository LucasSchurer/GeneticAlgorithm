using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationHandler : MonoBehaviour
{
    [SerializeField]
    private Creature _baseCreature;
    [SerializeField]
    private int _populationSize = 1;
    private Creature[] _population;
    private Chromosome[] _newGenerationChromosomes;
    [SerializeField]
    private int _currentGeneration = 0;
    [SerializeField]
    private GameObject _generationCreaturesHolder;
    [SerializeField]
    private bool shouldUpdateGeneration = false;
    [SerializeField]
    private float _timeToStartNewGeneration = 10f;
    [SerializeField]
    private float _newGenerationTimer = 0f;
    [SerializeField]
    private float _populationFitness = 0;
    private float[] _populationSumFitness;

    [SerializeField]
    private Creature fittestCreature;

    [SerializeField]
    private int numberOfIterations = 0;

    private void Awake()
    {
        _population = new Creature[_populationSize];
        _newGenerationChromosomes = new Chromosome[_populationSize];
        _populationSumFitness = new float[_populationSize];
        _generationCreaturesHolder = new GameObject("Generation Creatures");
    }

    private void Start()
    {
        InitializePopulation();
        StartGeneration();
    }

    private void InitializePopulation()
    {
        for (int i = 0; i < _populationSize; i++)
        {
            _population[i] = Object.Instantiate(_baseCreature, _generationCreaturesHolder.transform);
            _population[i].gameObject.SetActive(true);
            _population[i].Spawn(true);
        }
    }

    private void Update()
    {
        if (shouldUpdateGeneration)
        {
            UpdateGeneration();
        }
    }

    private void StartGeneration()
    {
        if (_currentGeneration != 0)
        {
            /*return;*/
        }

        _currentGeneration++;
        _newGenerationTimer = _timeToStartNewGeneration;
        shouldUpdateGeneration = true;

        _generationCreaturesHolder.name = $"{_currentGeneration} Generation Creatures";

        foreach (Creature creature in _population)
        {
            creature.UpdateValues();
            numberOfIterations++;
        }

        fittestCreature = _population[0];
        UpdatePopulationFitness();

        UIManager.Instance.SetGeneration(_currentGeneration);
    }

    private void UpdateGeneration()
    {
        _newGenerationTimer -= Time.deltaTime;

        foreach (Creature creature in _population)
        {
            creature.UpdateCreature();
        }

        if (_newGenerationTimer <= 0f)
        {
            EndGeneration();
        }
    }

    private void EndGeneration()
    {
        shouldUpdateGeneration = false;

        Selection();

        StartGeneration();
    }

    /// <summary>
    /// Select the fittest creatures among the population
    /// </summary>
    private void Selection()
    {
        _population = _population.OrderBy(c => c._fitnessValue).ToArray();

        _newGenerationChromosomes[0] = fittestCreature.Chromosome;
        _newGenerationChromosomes[1] = fittestCreature.Chromosome;

        for (int i = 2; i < _populationSize; i++)
        {
            numberOfIterations++;

            // Selection
            int parentA = GetParentIndex();
            int parentB = GetParentIndex();

            // Crossover
            System.Tuple<Chromosome, Chromosome> offspring = Chromosome.Crossover(_population[GetParentIndex()].Chromosome, _population[GetParentIndex()].Chromosome);

            // Mutation
            offspring.Item1.Mutate();
            offspring.Item2.Mutate();

            _newGenerationChromosomes[i] = offspring.Item1;
            _newGenerationChromosomes[i + 1] = offspring.Item2;
            i++;
        }

        for (int i = 0; i < _populationSize; i++)
        {
            numberOfIterations++;
            _population[i].Chromosome = System.ObjectExtensions.Copy(_newGenerationChromosomes[i]);
        }

        fittestCreature = null;
    }

    private int GetParentIndex()
    {
        /*return Random.Range(Mathf.FloorToInt(_populationSize / 2), _populationSize - 1);*/

        float randomFitness = Random.Range(0, _populationFitness);

        int max = _populationSize - 1;
        int min = 0;
        int middle = Mathf.FloorToInt((min + max) / 2);

        int stop = 0;
        /*float fitnessRange = 0f;*/

        return BinarySearchParent(randomFitness, 0, _populationSize - 1);

        /*for (int i = _populationSize -1; i >= 0; i--)
        {
            numberOfIterations++;

            fitnessRange += _population[i]._fitnessValue;

            if (fitnessRange > randomFitness)
            {
                return i;
            }
        }*/

        /*if (randomFitness <= _populationSumFitness[min])
        {
            return min;
        }

        if (randomFitness <= _populationSumFitness[max] && randomFitness > _populationSumFitness[max - 1])
        {
            return max;
        }

        while (min < max && stop < _populationSize)
        {
            numberOfIterations++;

            if (randomFitness <= _populationSumFitness[middle] && randomFitness > _populationSumFitness[middle - 1])
            {
                return middle;
            }

            if (randomFitness > _populationSumFitness[middle])
            {
                min = middle;
            }

            if (randomFitness < _populationSumFitness[middle])
            {
                max = middle;
            }

            middle = Mathf.FloorToInt((min + max) / 2);
        }*/

        return 0;
    }

    private int ExponentialSearchParent(float desiredFitness)
    {
        int bound = 1;

        while (bound < _populationSize && _population[bound]._fitnessValue < desiredFitness)
        {
            numberOfIterations++;
            bound *= 2;
        }

        return BinarySearchParent(desiredFitness, bound / 2, Mathf.Min(bound + 1, _populationSize - 1));
    }

    private int BinarySearchParent(float desiredFitness, int min, int max)
    {
        int middle = Mathf.FloorToInt((min + max) / 2);

        if (desiredFitness <= _populationSumFitness[min])
        {
            return min;
        }

        if (desiredFitness <= _populationSumFitness[max] && desiredFitness > _populationSumFitness[max - 1])
        {
            return max;
        }

        while (min < max)
        {
            numberOfIterations++;

            if (desiredFitness <= _populationSumFitness[middle] && desiredFitness > _populationSumFitness[middle - 1])
            {
                return middle;
            }

            if (desiredFitness > _populationSumFitness[middle])
            {
                min = middle;
            }

            if (desiredFitness < _populationSumFitness[middle])
            {
                max = middle;
            }

            middle = Mathf.FloorToInt((min + max) / 2);
        }

        return 0;
    }

    private void Crossover()
    {

    }

    private void Mutation()
    {

    }

    private void UpdatePopulationFitness()
    {
        _population = _population.OrderBy(c => c._fitnessValue).ToArray();
        _populationFitness = 0;

        fittestCreature = _population[0];

        for (int i = 0; i < _populationSize; i++)
        {
            numberOfIterations++;

            _populationFitness += _population[i].UpdateFitness();
            _populationSumFitness[i] = _populationFitness;

            if (fittestCreature._fitnessValue < _population[i]._fitnessValue)
            {
                fittestCreature = _population[i];
            }
        }

        UIManager.Instance.SetPopulationFitness(_populationFitness, _populationFitness/_populationSize);
    }
}
