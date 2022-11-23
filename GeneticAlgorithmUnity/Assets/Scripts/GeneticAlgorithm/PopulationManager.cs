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

        private List<CreatureController> _creatures;

        public float[] populationPropertiesSum;

        public static PopulationManager Instance { get; private set; }
        public FitnessProperties FitnessProperties => _fitnessProperties;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                _fitnessProperties.BalancePropertiesWeights();
                _creatures = new List<CreatureController>();
            }

            DontDestroyOnLoad(gameObject);
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
            Vector3 position = _spawnPosition.position;
            position.x += Random.Range(-5f, 5f);
            position.z += Random.Range(-5f, 5f);

            _creatures.Add(Instantiate(_creaturePrefab, position, Quaternion.identity));
        }

        private void UpdatePopulationFitness()
        {
            populationPropertiesSum = new float[_fitnessProperties.Properties.Length];

            foreach (CreatureController creature in _creatures)
            {
                creature.UpdateFitnessPropertiesValues(_fitnessProperties.Properties);

                for (int i = 0; i < _fitnessProperties.Properties.Length; i++)
                {
                    populationPropertiesSum[i] += creature.fitnessPropertiesValues[i]; 
                }
            }

            foreach (CreatureController creature in _creatures)
            {
                creature.UpdateFitness(_fitnessProperties.Properties, populationPropertiesSum);
            }
        }
    } 
}
