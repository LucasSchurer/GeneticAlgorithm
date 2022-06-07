using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationHandler : MonoBehaviour
{
    [SerializeField]
    private Creature _baseCreature;
    [SerializeField]
    private int _populationSize = 1;
    [SerializeField]
    private Creature[] _population;
    [SerializeField]
    private Creature[] _newGenerationPopulation;
    [SerializeField]
    private int _currentGeneration = 0;
    [SerializeField]
    private GameObject _generationCreaturesHolder;
    [SerializeField]
    private GameObject _newGenerationCreaturesHolder;
    private Creature _bestGenerationCreature;
    [SerializeField]
    private bool shouldUpdateGeneration = false;
    [SerializeField]
    private float _timeToStartNewGeneration = 10f;
    [SerializeField]
    private float _newGenerationTimer = 0f;
    [SerializeField]
    private float _populationFitness = 0;

    [SerializeField]
    private Creature fittestCreature;

    private void Awake()
    {
        _population = new Creature[_populationSize];
        _generationCreaturesHolder = new GameObject("Generation Creatures");
        _newGenerationCreaturesHolder = new GameObject("New Generation Creatures");
    }

    private void Start()
    {
        StartGeneration();
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

        for (int i = 0; i < _populationSize; i++)
        {
            if (_currentGeneration == 1)
            {
                _population[i] = Object.Instantiate(_baseCreature, _generationCreaturesHolder.transform);
                _population[i].gameObject.SetActive(true);
            }

            _population[i].Spawn(_currentGeneration == 1);
        }

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

        /*UpdatePopulationFitness();*/

        Selection();

        StartGeneration();
    }

    private void InitializePopulation()
    {

    }

    /// <summary>
    /// Select the fittest creatures among the population
    /// </summary>
    private void Selection()
    {
        _newGenerationPopulation = new Creature[_populationSize];

        Creature fittestChildA = Instantiate(fittestCreature, _newGenerationCreaturesHolder.transform);
        Creature fittestChildB = Instantiate(fittestCreature, _newGenerationCreaturesHolder.transform);

        fittestChildA.Chromosome = System.ObjectExtensions.Copy(fittestCreature.Chromosome);
        fittestChildB.Chromosome = System.ObjectExtensions.Copy(fittestCreature.Chromosome);

        _newGenerationPopulation[0] = fittestChildA;
        _newGenerationPopulation[1] = fittestChildB;

        for (int i = 2; i < _populationSize; i++)
        {
            // Selection
            Creature parentA = GetParent();
            Creature parentB = GetParent();

            // Crossover
            System.Tuple<Chromosome, Chromosome> offspring = Chromosome.Crossover(parentA.Chromosome, parentB.Chromosome);

            Creature childA = Instantiate(parentA, _newGenerationCreaturesHolder.transform);
            Creature childB = Instantiate(parentB, _newGenerationCreaturesHolder.transform);
            childA.Chromosome = offspring.Item1;
            childB.Chromosome = offspring.Item2;

            // Mutation
            childA.Chromosome.Mutate();
            childB.Chromosome.Mutate();

            childA.ClearFitnessValue();
            childB.ClearFitnessValue();
            _newGenerationPopulation[i] = childA;
            _newGenerationPopulation[i + 1] = childB;
            i++;
        }

        for (int i = 0; i < _populationSize; i++)
        {
            Destroy(_population[i].gameObject);
            _population[i] = null;
            _population[i] = _newGenerationPopulation[i];
            _population[i].transform.parent = _generationCreaturesHolder.transform;
        }

        _newGenerationPopulation = null;

        fittestCreature = null;
    }

    private Creature GetParent()
    {
        float random = Random.Range(0, _populationFitness);
        float fitnessRange = 0;

        foreach (Creature possibleParent in _population)
        {
            fitnessRange += possibleParent.Fitness;

            if (fitnessRange > random)
            {
                return possibleParent;
            }
        }

        Debug.Log("Invalid Parent Returned");
        return null;
    }

    private void Crossover()
    {

    }

    private void Mutation()
    {

    }

    private void UpdatePopulationFitness()
    {
        _populationFitness = 0;

        foreach (Creature creature in _population)
        {
            _populationFitness += creature.UpdateFitness();

            if (fittestCreature == null)
            {
                fittestCreature = creature;
            }

            if (fittestCreature.Fitness < creature.Fitness)
            {
                fittestCreature = creature;
            }
        }

        UIManager.Instance.SetPopulationFitness(_populationFitness, _populationFitness/_populationSize);
    }
}
