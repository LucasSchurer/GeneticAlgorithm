using Game.Entities;
using Game.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public struct CreatureData
    {
        public int id;
        public int generation;
        public Dictionary<StatisticsType, float> statistics;
        public TraitIdentifier[] traits;
        public float fitness;
        public int[] parents;
        public List<int> children;
        public float[] fitnessPropertiesValues;
    } 
}
