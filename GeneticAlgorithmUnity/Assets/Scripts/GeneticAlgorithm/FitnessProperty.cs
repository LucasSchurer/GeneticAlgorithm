using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    [System.Serializable]
    public class FitnessProperty
    {
        [SerializeField]
        private Entities.StatisticsType _statisticType;
        [Range(0f, 1f)]
        [SerializeField]
        private float _weight;
        [SerializeField]
        private bool _inverse;

        public Entities.StatisticsType StatisticsType => _statisticType;
        public float Weight { get => _weight; set => _weight = Mathf.Clamp(value, 0f, 1f); }
        public bool Inverse => _inverse;

        public void UpdateWeightProportionally(float weightSum)
        {
            if (_weight != 0f && weightSum != 0f)
            {
                _weight = _weight / weightSum;
            }
        }
    } 
}

