using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    [System.Serializable]
    public class NonPersistentAttribute
    {
        [SerializeField]
        private AttributeType _type;
        [SerializeField]
        private float _maxValue;
        [SerializeField]
        private float _minValue;
        [SerializeField]
        private float _currentValue;

        public AttributeType Type => _type;
        public float MaxValue { set => _maxValue = value; get => _maxValue; }
        public float MinValue { set => _minValue = value; get => _minValue; }

        public float CurrentValue
        {
            set => _currentValue = Mathf.Clamp(value, _minValue, _maxValue);
            get => _currentValue;
        }

        public NonPersistentAttribute(PersistentAttribute pAttribute)
        {
            _type = pAttribute.Type;
            _maxValue = pAttribute.MaxValue;
            _minValue = pAttribute.MinValue;
            _currentValue = pAttribute.StartingValue;
        }

        public NonPersistentAttribute()
        {
            _type = AttributeType.None;
            _maxValue = 0;
            _minValue = 0;
            _currentValue = 0;
        }
    } 
}
