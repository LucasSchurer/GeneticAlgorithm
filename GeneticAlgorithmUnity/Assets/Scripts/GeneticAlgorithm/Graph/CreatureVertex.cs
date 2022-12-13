using Game.Entities;
using Game.Traits;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class CreatureVertex
    {
        public class Data
        {
            public int id;
            public int generation;
            public Dictionary<StatisticsType, float> statistics;
            public TraitIdentifier[] traits;
            public float fitness;
            public int[] parents;
            public int[] children;

            public void UpdateData(CreatureController creature)
            {
                id = creature.id;
                generation = creature.generation;

                statistics = creature.GetComponent<StatisticsController>().Copy();
                fitness = creature.fitness;
                traits = creature.GetComponent<EntityTraitController>().GetTraitsIdentifiers();
                
                if (creature.parents != null)
                {
                    parents = new int[creature.parents.Length];
                    Array.Copy(creature.parents, parents, creature.parents.Length);
                } 
                
                if (creature.children != null)
                {
                    children = new int[creature.children.Count];
                    Array.Copy(creature.children.ToArray(), children, creature.children.Count);
                }
            }


            public string ToJson()
            {
                return "";
            }

            public static Data FromJson(string json)
            {
                return new Data();
            }
        }

        public Data data;

        public CreatureVertex(CreatureController creature)
        {
            BuildData(creature);
        }

        private void BuildData(CreatureController creature)
        {
            data = new Data();
            data.UpdateData(creature);
        }
    } 
}
