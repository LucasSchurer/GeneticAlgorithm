using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    [CreateAssetMenu(fileName = "FitnessProperties", menuName = "Genetic Algorithm/Fitness Properties")]
    public class FitnessProperties : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField]
        private FitnessProperty[] _properties;
        [SerializeField]
        private bool _autoBalance = true;

        [Header("Debug")]
        [SerializeField]
        private float _weightSum = 0f;

        public FitnessProperty[] Properties => _properties;

        private void UpdateWeightSum()
        {
            _weightSum = 0f;

            for (int i = 0; i < _properties.Length; i++)
            {
                _weightSum += _properties[i].Weight;
            }
        }

        public void BalancePropertiesWeights()
        {
            UpdateWeightSum();
            float balancedWeightSum = 0f;

            for (int i = 0; i < _properties.Length; i++)
            {
                _properties[i].UpdateWeightProportionally(_weightSum);

                balancedWeightSum += _properties[i].Weight;
            }

            _weightSum = balancedWeightSum;
        }

        public void SetWeights(float value)
        {
            for (int i = 0; i < _properties.Length; i++)
            {
                _properties[i].Weight = value;
            }

            UpdateWeightSum();
        }

        public void OnValidate()
        {
            if (_autoBalance)
            {
                BalancePropertiesWeights();
            } else
            {
                UpdateWeightSum();
            }
        }
    } 
}
