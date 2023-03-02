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
        
/*        [XmlIgnore]
        public Dictionary<StatisticsType, float> statistics;
        [XmlArray("Statistics")]
        [XmlArrayItem("Statistic")]
        public List<SerializableDictionary<StatisticsType, float>> serializableStatistics => SerializableDictionary<StatisticsType, float>.BuildListFromDictionary(statistics);*/
        
        private TraitIdentifier[] _traits;
        private Fitness _fitness;
        public int[] parents;
        private CreatureData[] _parents;
        public List<int> children;
        private List<CreatureData> _children;
        public float[] fitnessPropertiesValues;
        public BaseEnemyChromosome chromosome;
        public TraitIdentifier[] Traits { get => _traits; set => _traits = value; }
        public Fitness Fitness => _fitness;
        public CreatureData[] Parents { get => _parents; set => _parents = value; }
        public List<CreatureData> Children => _children;

        public CreatureData(CreatureController creature, int[] parents)
        {
        }

        public CreatureData()
        {
            _children = new List<CreatureData>();
            _fitness = new Fitness();
        }
    }
}
