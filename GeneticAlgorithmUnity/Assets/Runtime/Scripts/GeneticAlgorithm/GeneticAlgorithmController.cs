using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class GeneticAlgorithmController : MonoBehaviour, IEventListener
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
        [SerializeField]
        private Weapons.WeaponManager.WeaponHolder _teamWeapon = Weapons.WeaponManager.WeaponHolder.Team1;
        [SerializeField]
        private Traits.TraitManager.Team _traitTeam;

        [SerializeField]
        private bool _addTraitsToElitist;

        public enum SelectionMethod
        { 
            Roulette,
            Tournament
        }

        [SerializeField]
        private SelectionMethod _selection;
        public Traits.TraitManager.Team Team => _traitTeam;

        [SerializeField]
        private int _elitism = 0;

        [SerializeField]
        private int _team;
        [SerializeField]
        private int _generationsNeededToAddTraits = 1;
        [SerializeField]
        private int _tournamentSize = 1;
        [SerializeField]
        private bool _cloneEverything = false;
        [SerializeField]
        private bool _addTraitsToCloneEverything = false;
        [SerializeField]
        [Tooltip("If set to < 0 there wont be a limit to the amount of traits a creature can receive")]
        private int _maxTraits = 5;

        private GeneticAlgorithmData _geneticAlgorithmData;
        private PopulationController _populationController;
        private GenerationController _generationController;

        public bool AddTraitsToElitist => _addTraitsToElitist;
        public bool CloneEverything => _cloneEverything;
        public bool AddTraitsToCloneEverything => _addTraitsToCloneEverything;

        public int Elitism => _elitism;
        public SelectionMethod Selection => _selection;
        public int TournamentSize => _tournamentSize;
        public int GenerationsNeededToAddTraits => _generationsNeededToAddTraits;

        public Weapons.WeaponManager.WeaponHolder WeaponTeam => _teamWeapon;

        public int MaxTraits => _maxTraits;
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

        private void Awake()
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
            _geneticAlgorithmData.team = _team;
            _geneticAlgorithmData.properties = _fitnessProperties.Properties;
            _geneticAlgorithmData.traitChangePositiveThreshold = _positiveTraitWeightChangeThreshold;
            _geneticAlgorithmData.traitChangeNegativeThreshold = _negativeTraitWeightChangeThreshold;
            _geneticAlgorithmData.traitChangeAmount = _traitWeightChange;
            _geneticAlgorithmData.mutationRate = _mutationRate;
            _geneticAlgorithmData.mutateIndividually = _mutateIndividually;
            _geneticAlgorithmData.parentsAmount = _parentsAmount;
            _geneticAlgorithmData.selectionMethod = _selection;
            _geneticAlgorithmData.tournamentSize = _tournamentSize;
            _geneticAlgorithmData.elitism = _elitism;
            _geneticAlgorithmData.generationsNeededToAddTrait = _generationsNeededToAddTraits;
            _geneticAlgorithmData.cloneEverything = _cloneEverything;
            _geneticAlgorithmData.addTraitsToCloneEverything = _addTraitsToCloneEverything;
            _geneticAlgorithmData.maxTraitAmount = _maxTraits;
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
