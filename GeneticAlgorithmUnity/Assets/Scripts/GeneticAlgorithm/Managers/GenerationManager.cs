using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.GA
{
    public class GenerationManager
    {
        private Dictionary<int, GenerationData> _generations;

        public GenerationData CurrentGenerationData => _generations[_generations.Count];
        public GenerationData[] GenerationsArray => _generations.Values.ToArray();

        public GenerationManager()
        {
            _generations = new Dictionary<int, GenerationData>();
        }

        public void AddGenerationData(GenerationData data)
        {
            _generations.TryAdd(data.number, data);
        }

        public void AddCreatureToGeneration(CreatureController creature)
        {
            if (creature.data.generation >= _generations.Count)
            {
                _generations.Add(creature.data.generation, new GenerationData());
            }

            _generations[creature.data.generation].AddCreature(creature);
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
    } 
}
