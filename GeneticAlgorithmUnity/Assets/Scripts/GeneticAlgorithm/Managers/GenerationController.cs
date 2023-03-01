using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.GA
{
    public class GenerationController
    {
        private int _currentGeneration = 0;
        private int _currentCreatureId = 0;
        
        private Dictionary<int, GenerationData> _generations;
        public List<GenerationData> Generations => _generations.Values.ToList();
        public GenerationData CurrentGenerationData => _generations[_generations.Count];
        public GenerationController()
        {
            _generations = new Dictionary<int, GenerationData>();
        }
        public int CurrentGeneration => _currentGeneration;

        public void CreateGeneration()
        {
            _currentGeneration++;

            if (_currentGeneration == 1)
            {
                CreateFreshGeneration();
            } else
            {
                CreateGeneration(CurrentGenerationData.number - 1);
            }
        }

        /// <summary>
        /// Method that generates a new set of 
        /// creatures, using selection, crossover
        /// and mutation methods.
        /// The newly generated creatures will be part of the
        /// last (and current) generation.
        /// </summary>
        private void CreateGeneration(int parentsGeneration)
        {

        }

        /// <summary>
        /// Creates a generation without any data from
        /// parents. 
        /// </summary>
        private void CreateFreshGeneration()
        {
            for (int i = 0; i < Managers.WaveManager.Instance.waveSettings.enemiesPerWave; i++)
            {
                AddCreatureDataToGeneration(CreateCreatureData());
            }
        }

        private CreatureData CreateCreatureData(int[] parents = null)
        {
            CreatureData data = new CreatureData();
            data.id = _currentCreatureId;
            data.generation = _currentGeneration;
            data.isDead = false;
            data.parents = parents;

            _currentCreatureId++;

            return data;
        }

        public CreatureData[] GetCurrentGenerationCreaturesData()
        {
            return CurrentGenerationData.creatures.Values.ToArray();
        }

        public void AddGenerationData(GenerationData data)
        {
            _generations.TryAdd(data.number, data);
        }

        private void AddCreatureDataToGeneration(CreatureData data)
        {
            _generations.TryAdd(data.generation, new GenerationData());

            _generations[data.generation].AddCreatureData(data);
        }

        public GenerationData GetGenerationData(int generation)
        {            
            if (generation < _generations.Count)
            {
                return _generations[generation];
            }
            else
            {
                throw new System.Exception("Generation out of bounds");
            }
        }

        #region SELECTION METHODS

        #endregion
    }
}
