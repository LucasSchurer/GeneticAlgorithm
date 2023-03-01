using Game.Entities;
using Game.Traits;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Game.GA
{
    [XmlRoot("Creature")]
    public class CreatureData
    {
        public int id;
        public int generation;
        public bool isDead = false;
        
        [XmlIgnore]
        public Dictionary<StatisticsType, float> statistics;
        [XmlArray("Statistics")]
        [XmlArrayItem("Statistic")]
        public List<SerializableDictionary<StatisticsType, float>> serializableStatistics => SerializableDictionary<StatisticsType, float>.BuildListFromDictionary(statistics);
        
        public TraitIdentifier[] traits;
        public float fitness;
        public int[] parents;
        public List<int> children;
        public float[] fitnessPropertiesValues;
        public BaseEnemyChromosome chromosome;

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

            creature.data = this;
        }

        public CreatureData()
        { }
    }
}
