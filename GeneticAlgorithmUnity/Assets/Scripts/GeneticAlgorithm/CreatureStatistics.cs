using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;
using Game.Traits;

namespace Game.GA
{
    public class CreatureStatistics
    {
        public Dictionary<StatisticsController.Type, float> baseStatistics;
        public float fitness;
        public TraitIdentifier[] traits;
        public int generation;
    } 
}
