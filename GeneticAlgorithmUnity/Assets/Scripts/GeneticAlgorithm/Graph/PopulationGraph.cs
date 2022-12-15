using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class PopulationGraph
    {
        private List<Dictionary<int, CreatureVertex>> _creatures;

        public int CurrentGeneration => _creatures.Count;

        public PopulationGraph()
        {
            _creatures = new List<Dictionary<int, CreatureVertex>>();
        }

        public void CreateAndAddVertex(CreatureController creature)
        {
            AddVertex(new CreatureVertex(creature));
        }

        public void AddVertex(CreatureVertex vertex)
        {
            if (vertex.data.generation >= _creatures.Count)
            {
                _creatures.Add(new Dictionary<int, CreatureVertex>());
            }

            _creatures[vertex.data.generation].TryAdd(vertex.data.id, vertex);
        }

        public Dictionary<int, CreatureVertex> GetGeneration(int generation)
        {
            if (generation < _creatures.Count)
            {
                return _creatures[generation];
            } else
            {
                return null;
            }
        }

        public CreatureVertex GetVertex(int generation, int id)
        {
            return _creatures[generation][id];
        }

        public void WriteJson()
        {

        }
    } 
}
