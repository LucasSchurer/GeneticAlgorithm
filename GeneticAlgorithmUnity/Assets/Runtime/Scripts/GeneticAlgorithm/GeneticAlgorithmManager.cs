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
        [Tooltip("Amount of parents selected to generate a new creature")]
        [Range(1, 10)]
        [SerializeField]
        private int _parentsAmount = 1;
        /// <summary>
        /// Property that controls if the mutation rate will be used only one time to all genes
        /// or will be calculated for each gene individually
        /// </summary>
        [SerializeField]
        private bool _mutateIndividually = false;
        [SerializeField]
        private int _traitSelectionAmount = 1;
        [SerializeField]
        [Range(0f, 1f)]
        private float _traitSelectionDumbness = 0f;
        [SerializeField]
        private float _negativeTraitWeightChangeThreshold = 0.5f;
        [SerializeField]
        private float _positiveTraitWeightChangeThreshold = 0.5f;
        [SerializeField]
        private float _traitWeightChange = 0.01f;

        private GeneticAlgorithmData _geneticAlgorithmData;
        private PopulationController _populationController;
        private GenerationController _generationController;

        public GeneticAlgorithmData GeneticAlgorithmData => _geneticAlgorithmData;
        public PopulationController PopulationController => _populationController;
        public GenerationController GenerationController => _generationController;
        public FitnessProperties FitnessProperties => _fitnessProperties;
        public float MutationRate => _mutationRate;
        public int ParentsAmount => _parentsAmount;
        public bool MutateIndividually => _mutateIndividually;
        public int TraitSelectionAmount => _traitSelectionAmount;
        public float TraitSelectionDumbness => _traitSelectionDumbness;
        public float NegativeTraitWeightChangeThreshold => _negativeTraitWeightChangeThreshold;
        public float PositiveTraitWeightChangeThreshold => _positiveTraitWeightChangeThreshold;
        public float TraitWeightChange => _traitWeightChange;

        protected override void SingletonAwake()
        {
            _geneticAlgorithmData = new GeneticAlgorithmData();
            _populationController = GetComponent<PopulationController>();
            _generationController = GetComponent<GenerationController>();
        }
        
        private void GenerateXMLOnWaveEnd(ref GameEventContext ctx)
        {
            _geneticAlgorithmData.generations = _generationController.Generations;
            _geneticAlgorithmData.traitSelectionAmount = _traitSelectionAmount;
            _geneticAlgorithmData.traitSelectionDumbness = _traitSelectionDumbness;
            _geneticAlgorithmData.ToXML();
        }

        public void StartListening()
        {
            Managers.GameManager.Instance.GetEventController()?.AddListener(GameEventType.OnWaveEnd, GenerateXMLOnWaveEnd, EventExecutionOrder.After);
        }

        public void StopListening()
        {
            Managers.GameManager.Instance.GetEventController()?.RemoveListener(GameEventType.OnWaveEnd, GenerateXMLOnWaveEnd, EventExecutionOrder.After);
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
