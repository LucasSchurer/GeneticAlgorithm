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
        private float _mutationRate = 0.15f;

        private List<CreatureController> _creatures;

        public float[] populationFitnessPropertiesValuesSums;
        public float populationFitness;

        private void Awake()
        {
            _fitnessProperties.BalancePropertiesWeights();
            _creatures = new List<CreatureController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                UpdatePopulationFitness();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Spawn();
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
                Spawn();
            }
        }

        private void Spawn()
        {
            _creatures.Add(Instantiate(_creaturePrefab, GetSpawnPosition(), Quaternion.identity));
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 position = _spawnPosition.position;
            position.x += Random.Range(-5f, 5f);
            position.z += Random.Range(-5f, 5f);

            return position;
        }

        private void GenerateNewPopulation()
        {
            UpdatePopulationFitness();

            CreatureController[] newCreatures = new CreatureController[_creatures.Count];

            for (int i = 0; i < newCreatures.Length; i+=2)
            {
                // Selection
                int parentA = RouletWheelSelection();
                int parentB = RouletWheelSelection();

                // Crossover
                Chromosome[] offspring = Chromosome.Crossover(_creatures[parentA].chromosome, _creatures[parentB].chromosome);

                // Mutation
                offspring[0].Mutate();
                offspring[0].Mutate();

                newCreatures[i] = Instantiate(_creaturePrefab);
                newCreatures[i + 1] = Instantiate(_creaturePrefab);

                newCreatures[i].SetChromosome((BaseEnemyChromosome)offspring[0], _mutationRate);
                newCreatures[i + 1].SetChromosome((BaseEnemyChromosome)offspring[1], _mutationRate);
            }

            for (int i = 0; i < newCreatures.Length; i++)
            {
                Destroy(_creatures[i].gameObject);
                _creatures[i] = newCreatures[i];
                _creatures[i].gameObject.SetActive(true);
                _creatures[i].transform.position = GetSpawnPosition();
            }
        }

        private int RouletWheelSelection()
        {
            float randomFitness = Random.Range(0, populationFitness);
            float fitnessRange = 0f;

            for (int i = 0; i < _creatures.Count; i++)
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
