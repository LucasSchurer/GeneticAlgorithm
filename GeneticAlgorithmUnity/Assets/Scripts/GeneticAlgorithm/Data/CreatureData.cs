using Game.Entities;
using Game.Traits;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                
        private TraitIdentifier[] _traits;
        private Fitness _fitness;
        public int[] parents;
        private CreatureData[] _parents;
        public List<int> children;
        private List<CreatureData> _children;
        public float[] fitnessPropertiesValues;
        [XmlIgnore]
        public BaseEnemyChromosome chromosome;
        public TraitIdentifier[] Traits { get => _traits; set => _traits = value; }
        [XmlElement]
        public Fitness Fitness { get => _fitness; set => _fitness = value; }
        [XmlIgnore]
        public CreatureData[] Parents { get => _parents; set => _parents = value; }
        [XmlIgnore]
        public List<CreatureData> Children => _children;
        // Only used to XML
        public List<int> ChildrenID => _children.Select(c => c.id).ToList();
        // Only used to XML
        public List<int> ParentsID => _parents != null ? _parents.Select(p => p.id).ToList() : new List<int>();

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
