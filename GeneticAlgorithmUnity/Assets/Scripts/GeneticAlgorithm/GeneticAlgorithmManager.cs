using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class GeneticAlgorithmManager : Singleton<GeneticAlgorithmManager>
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
            _generationController = new GenerationController();
        }
    }
}
