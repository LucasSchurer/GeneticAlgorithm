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

        private void Awake()
        {
            _fitnessProperties.BalancePropertiesWeights();
            _creatures = new CreatureController[_spawnAmount];
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
                }
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
            }

            for (int i = 0; i < newCreatures.Length; i++)
            {
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
