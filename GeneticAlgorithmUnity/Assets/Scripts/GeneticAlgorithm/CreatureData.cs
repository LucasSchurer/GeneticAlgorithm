using Game.Entities;
using Game.Traits;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class CreatureData
    {
        public int id;
        public int generation;
        public Dictionary<StatisticsType, float> statistics;
        public TraitIdentifier[] traits;
        public float fitness;
        public int[] parents;
        public List<int> children;
        public float[] fitnessPropertiesValues;

        public string ToJson()
        {
            return "";
        }

        public void FromJson()
        {

        }

        public CreatureData(CreatureController creature, int[] parents)
        {
            children = new List<int>();
            
            if (parents != null)
            {
                this.parents = new int[parents.Length];
                Array.Copy(parents, this.parents, this.parents.Length);
            }

            StatisticsController statisticsController = creature.GetComponent<StatisticsController>();

            if (statisticsController)
            {
                statistics = statisticsController._statistics;
            }

            EntityTraitController traitController = creature.GetComponent<EntityTraitController>();

            if (traitController)
            {
                traits = traitController.GetTraitsIdentifiers();
            }
        }
    } 
}
