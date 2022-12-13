using Game.Entities;
using Game.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class Vertex
    {
        private int _id;
        private int _generation;
        public int[] parents;
        private List<int> _children;
        public CreatureStatistics statistics;

        public int Id => _id;
        public int Generation => _generation;

        public Vertex(Graph graph, CreatureController creature)
        {
            _id = creature.id;
            _generation = creature.generation;

            parents = new int[creature.parents.Length];

            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = creature.parents[i];
            }

            _children = new List<int>();

            statistics = new CreatureStatistics();
            statistics.baseStatistics = creature.GetComponent<StatisticsController>().Copy();
            statistics.fitness = creature.fitness;
            statistics.generation = Generation;
            statistics.traits = creature.GetComponent<EntityTraitController>().GetTraitsIdentifiers();
        }
    } 
}
