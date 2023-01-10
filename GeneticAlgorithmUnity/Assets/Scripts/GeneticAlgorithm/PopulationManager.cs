using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;

namespace Game.GA
{
    public class PopulationManager : MonoBehaviour
    {
        public static PopulationManager Instance { get; private set; }

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
        private FitnessProperties _fitnessProperties;
        [SerializeField]
        private CreatureController _creaturePrefab;

        [Header("Settings")]        
        [SerializeField]
        private float _mutationRate = 0.15f;

        private CreatureController[] _creatures;
        private List<CreatureController> _creaturesRequest;

        public float[] populationMaxPropertiesValues;
        public float populationFitness;

        private PopulationGraph _populationGraph;
        private int _currentCreatureId = 1;
        private int _currentGeneration = 0;

        public void Initialize(int initialPopulationSize)
        {
            _fitnessProperties.BalancePropertiesWeights();
            _creatures = new CreatureController[initialPopulationSize];
            _creaturesRequest = new List<CreatureController>();
            _populationGraph = new PopulationGraph();

            GetComponent<UI.GraphVisualizer>()?.SetGraph(_populationGraph);

            GenerateInitialPopulation();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                UpdatePopulationFitness();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                GenerateNewPopulation();
            }
        }

        private void GenerateInitialPopulation()
        {
            for (int i = 0; i < _creatures.Length; i++)
            {
                _creatures[i] = Instantiate(_creaturePrefab);
                _creatures[i].Initialize(_mutationRate, true);
                _creatures[i].gameObject.SetActive(false);
                _creatures[i].data.id = _currentCreatureId;
                _creatures[i].data.generation = _currentGeneration;
                _currentCreatureId++;

                _populationGraph.CreateAndAddVertex(_creatures[i]);

                _creaturesRequest.Add(_creatures[i]);
            }

            _currentGeneration++;
        }

        private void GenerateNewPopulation()
        {
            UpdatePopulationFitness();

            CreatureController[] newCreatures = new CreatureController[_creatures.Length];

            for (int i = 0; i < newCreatures.Length; i+=2)
            {
                // Selection
                int parentA = RouletteWheelSelection();
                int parentB = RouletteWheelSelection();

                // Crossover
                Chromosome[] offspring = Chromosome.Crossover(_creatures[parentA].chromosome, _creatures[parentB].chromosome);

                // Mutation
                offspring[0].Mutate();
                offspring[1].Mutate();

                newCreatures[i] = Instantiate(_creaturePrefab);
                newCreatures[i + 1] = Instantiate(_creaturePrefab);

                newCreatures[i].Initialize(_mutationRate, false, (BaseEnemyChromosome)offspring[0]);
                newCreatures[i + 1].Initialize(_mutationRate, false, (BaseEnemyChromosome)offspring[1]);

                newCreatures[i].data.generation = _currentGeneration;
                newCreatures[i + 1].data.generation = _currentGeneration;

                newCreatures[i].data.id = _currentCreatureId;
                newCreatures[i + 1].data.id = _currentCreatureId + 1;

                _currentCreatureId += 2;

                newCreatures[i].data.parents = new int[2] { _creatures[parentA].data.id, _creatures[parentB].data.id };
                newCreatures[i + 1].data.parents = new int[2] { _creatures[parentA].data.id, _creatures[parentB].data.id };

                _creatures[parentA].data.children.Add(newCreatures[i].data.id);
                _creatures[parentA].data.children.Add(newCreatures[i + 1].data.id);

                _creatures[parentB].data.children.Add(newCreatures[i].data.id);
                _creatures[parentB].data.children.Add(newCreatures[i + 1].data.id);
            }

            _currentGeneration++;
            _creaturesRequest.Clear();

            for (int i = 0; i < newCreatures.Length; i++)
            {
                Destroy(_creatures[i].gameObject);
                _creatures[i] = newCreatures[i];
                _creatures[i].gameObject.SetActive(false);
                _populationGraph.CreateAndAddVertex(_creatures[i]);

                _creaturesRequest.Add(_creatures[i]);
            }
        }

        private int RouletteWheelSelection()
        {
            float randomFitness = Random.Range(0, populationFitness);
            float fitnessRange = 0f;

            for (int i = 0; i < _creatures.Length; i++)
            {
                fitnessRange += _creatures[i].data.fitness;

                if (fitnessRange > randomFitness)
                {
                    return i;
                }
            }

            return 0;
        }

        private void UpdatePopulationFitness()
        {
            populationMaxPropertiesValues = new float[_fitnessProperties.Properties.Length];
            populationFitness = 0f;

            for (int i = 0; i < populationMaxPropertiesValues.Length; i++)
            {
                populationMaxPropertiesValues[i] = 0;
            }

            foreach (CreatureController creature in _creatures)
            {
                creature.UpdateFitnessPropertiesValues(_fitnessProperties.Properties);

                for (int i = 0; i < _fitnessProperties.Properties.Length; i++)
                {
                    if (creature.data.fitnessPropertiesValues[i] > populationMaxPropertiesValues[i])
                    {
                        populationMaxPropertiesValues[i] = creature.data.fitnessPropertiesValues[i];
                    }
                }
            }

            foreach (CreatureController creature in _creatures)
            {
                creature.UpdateFitness(_fitnessProperties.Properties, populationMaxPropertiesValues);
                populationFitness += creature.data.fitness;
            }
        }

        public CreatureController RequestCreature()
        {
            if (_creaturesRequest.Count > 0)
            {
                int index = Random.Range(0, _creaturesRequest.Count);

                CreatureController creature = _creaturesRequest[index];
                _creaturesRequest.RemoveAt(index);

                return creature;
            }

            return null;
        }
    } 
}
