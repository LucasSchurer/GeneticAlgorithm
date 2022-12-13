using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;

namespace Game.GA
{
    public class PopulationManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private FitnessProperties _fitnessProperties;
        [SerializeField]
        private CreatureController _creaturePrefab;
        [SerializeField]
        private Transform _spawnPosition;

        [Header("Settings")]
        [SerializeField]
        private bool _spawnOnStart = false;
        [SerializeField]
        private int _spawnAmount = 4;
        [SerializeField]
        private float _mutationRate = 0.15f;

        private CreatureController[] _creatures;

        public float[] populationFitnessPropertiesValuesSums;
        public float populationFitness;

        private PopulationGraph _populationGraph;
        private int _currentCreatureId = 1;
        private int _currentGeneration = 0;

        private void Awake()
        {
            _fitnessProperties.BalancePropertiesWeights();
            _creatures = new CreatureController[_spawnAmount];
            _populationGraph = new PopulationGraph();

            GetComponent<UI.GraphVisualizer>()?.SetGraph(_populationGraph);
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

        private void Start()
        {
            if (_spawnOnStart)
            {
                for (int i = 0; i < _spawnAmount; i++)
                {
                    _creatures[i] = Instantiate(_creaturePrefab);
                    _creatures[i].Initialize(_mutationRate, true);
                    _creatures[i].gameObject.SetActive(true);
                    _creatures[i].transform.position = GetSpawnPosition();
                    _creatures[i].id = _currentCreatureId;
                    _creatures[i].generation = _currentGeneration;
                    _currentCreatureId++;
                }

                _currentGeneration++;
            }
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 position = _spawnPosition.position;
            position.x += Random.Range(-10f, 10f);
            position.z += Random.Range(-10f, 10f);

            return position;
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

                newCreatures[i].generation = _currentGeneration;
                newCreatures[i + 1].generation = _currentGeneration;

                newCreatures[i].id = _currentCreatureId;
                newCreatures[i + 1].id = _currentCreatureId + 1;

                _currentCreatureId += 2;

                newCreatures[i].parents = new int[2] { _creatures[parentA].id, _creatures[parentB].id };
                newCreatures[i + 1].parents = new int[2] { _creatures[parentA].id, _creatures[parentB].id };

                _creatures[parentA].children.Add(newCreatures[i].id);
                _creatures[parentA].children.Add(newCreatures[i + 1].id);

                _creatures[parentB].children.Add(newCreatures[i].id);
                _creatures[parentB].children.Add(newCreatures[i + 1].id);
            }

            _currentGeneration++;

            for (int i = 0; i < newCreatures.Length; i++)
            {
                _populationGraph.CreateAndAddVertex(_creatures[i]);

                Destroy(_creatures[i].gameObject);
                _creatures[i] = newCreatures[i];
                _creatures[i].gameObject.SetActive(true);
                _creatures[i].transform.position = GetSpawnPosition();
            }
        }

        private int RouletteWheelSelection()
        {
            float randomFitness = Random.Range(0, populationFitness);
            float fitnessRange = 0f;

            for (int i = 0; i < _creatures.Length; i++)
            {
                fitnessRange += _creatures[i].fitness;

                if (fitnessRange > randomFitness)
                {
                    return i;
                }
            }

            return 0;
        }

        private void UpdatePopulationFitness()
        {
            populationFitnessPropertiesValuesSums = new float[_fitnessProperties.Properties.Length];
            populationFitness = 0f;

            foreach (CreatureController creature in _creatures)
            {
                creature.UpdateFitnessPropertiesValues(_fitnessProperties.Properties);

                for (int i = 0; i < _fitnessProperties.Properties.Length; i++)
                {
                    populationFitnessPropertiesValuesSums[i] += creature.fitnessPropertiesValues[i]; 
                }
            }

            foreach (CreatureController creature in _creatures)
            {
                creature.UpdateFitness(_fitnessProperties.Properties, populationFitnessPropertiesValuesSums);
                populationFitness += creature.fitness;
            }
        }
    } 
}
