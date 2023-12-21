using UnityEngine;
using Game.Entities.Shared;
using System.Runtime.Serialization;

namespace Game.GA
{
    [System.Serializable]
    [DataContract(Name = "Property", Namespace = "")]
    public class FitnessProperty
    {
        [DataMember(Name = "StatisticType")]
        [SerializeField]
        private StatisticsType _statisticType;
        [Range(0f, 1f)]
        [SerializeField]
        [DataMember(Name = "Weight")]
        private float _weight;
        [SerializeField]
        [DataMember(Name = "Inverse")]
        private bool _inverse;

        public StatisticsType StatisticsType => _statisticType;
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

