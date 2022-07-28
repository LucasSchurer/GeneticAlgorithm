using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

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
            _population[i].Chromosome = _newGenerationChromosomes[i].Copy();
        }

        fittestCreature = null;
    }

    private int GetParentIndex()
    {
        float randomFitness = StaticRandom.RandomFloat(0, _populationFitness);

        int max = _populationSize - 1;
        int min = 0;
        int middle = Mathf.FloorToInt((min + max) / 2);

        int stop = 0;
        float fitnessRange = 0f;

        for (int i = _populationSize - 1; i >= 0; i--)
        {
            numberOfIterations++;

            fitnessRange += _population[i]._fitnessValue;

            if (fitnessRange > randomFitness)
            {
                return i;
            }
        }

        return 0;
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
