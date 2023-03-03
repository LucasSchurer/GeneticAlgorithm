using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class GeneticAlgorithmManager : Singleton<GeneticAlgorithmManager>, IEventListener
    {
        [SerializeField]
        private FitnessProperties _fitnessProperties;
        [SerializeField]
        private float _mutationRate = 0.15f;

        private GeneticAlgorithmData _geneticAlgorithmData;
        private PopulationController _populationController;
        private GenerationController _generationController;

        public GeneticAlgorithmData GeneticAlgorithmData => _geneticAlgorithmData;
        public PopulationController PopulationController => _populationController;
        public GenerationController GenerationController => _generationController;
        public FitnessProperties FitnessProperties => _fitnessProperties;
        public float MutationRate => _mutationRate;
        
        protected override void SingletonAwake()
        {
            _geneticAlgorithmData = new GeneticAlgorithmData();
            _populationController = GetComponent<PopulationController>();
            _generationController = GetComponent<GenerationController>();
        }
        
        private void GenerateXMLOnWaveEnd(ref GameEventContext ctx)
        {
            _geneticAlgorithmData.generations = _generationController.Generations;
            _geneticAlgorithmData.ToXML();
        }

        public void StartListening()
        {
            Managers.GameManager.Instance.eventController?.AddListener(GameEventType.OnWaveEnd, GenerateXMLOnWaveEnd, EventExecutionOrder.After);
        }

        public void StopListening()
        {
            Managers.GameManager.Instance.eventController?.RemoveListener(GameEventType.OnWaveEnd, GenerateXMLOnWaveEnd, EventExecutionOrder.After);
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }
    }
}
