using Game.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Game.GA
{
    public class Fitness
    {
        private float _value;
        private Dictionary<StatisticsType, PartialValue> _partialValues;
        public float Value => _value;
        public PartialValue[] PartialValues => _partialValues.Values.ToArray();
        
        public Fitness()
        {
            FitnessProperty[] properties = GeneticAlgorithmManager.Instance.FitnessProperties.Properties;

            _partialValues = new Dictionary<Entities.StatisticsType, PartialValue>();

            foreach (FitnessProperty property in properties)
            {
                _partialValues.TryAdd(property.StatisticsType, new PartialValue(property));
            }
        }

        /// <summary>
        /// Update all raw values contained in partial values, using the argument passed
        /// to retrieve each value.
        /// </summary>
        /// <param name="controller"></param>
        public void UpdateRawFitnessValue(StatisticsController controller)
        {            
            foreach (PartialValue partialValue in _partialValues.Values)
            {
                partialValue.RawValue = controller.GetStatistic(partialValue.Type);
            }
        }

        public void UpdateFitness(Dictionary<StatisticsType, float> maxValues)
        {
            _value = 0;

            foreach (PartialValue partialValue in _partialValues.Values)
            {
                if (maxValues.TryGetValue(partialValue.Type, out float maxValue))
                {
                    partialValue.MaxValue = maxValue;

                    _value += partialValue.NormalizedValue;
                }
            }
        }

        public class PartialValue
        {
            private FitnessProperty _property;
            private StatisticsType _type;
            private float _rawValue = 0;
            private float _normalizedValue = 0;
            private float _maxValue = 0;

            public float NormalizedValue => _normalizedValue;
            public float RawValue { get => _rawValue; set => _rawValue = value; }
            public float MaxValue { get => _maxValue; set => SetMaxValue(value); }
            public StatisticsType Type { get => _type; set => _type = value; }

            public PartialValue(FitnessProperty property)
            {
                _property = property;
                _type = _property.StatisticsType;
            }

            private void SetMaxValue(float maxValue)
            {
                _maxValue = maxValue;

                UpdateNormalizedValue();
            }

            private void UpdateNormalizedValue()
            {
                if (_maxValue == 0)
                {
                    if (_property.Inverse)
                    {
                        _normalizedValue = 1 * _property.Weight;
                        return;
                    }
                }

                _normalizedValue = _rawValue > 0 ? (_rawValue / _maxValue) : 0;

                if (_property.Inverse)
                {
                    _normalizedValue = 1 - _normalizedValue;
                }

                _normalizedValue *= _property.Weight;
            }
        }
    } 
}
