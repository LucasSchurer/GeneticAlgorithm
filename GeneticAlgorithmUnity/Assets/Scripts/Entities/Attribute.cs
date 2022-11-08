using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public abstract class Attribute : ScriptableObject
    {
        [SerializeField]
        private float _currentValue;
        [SerializeField]
        private float _maximumValue;
        [SerializeField]
        private float _minimumValue;

        public float CurrentValue => _currentValue;
        public float MaximumValue => _maximumValue;
        public float MinimumValue => _minimumValue;

        public virtual void ModifyValue(ref float value, float change)
        {
            value = Mathf.Clamp(value + change, _minimumValue, _maximumValue);
        }
    } 
}
