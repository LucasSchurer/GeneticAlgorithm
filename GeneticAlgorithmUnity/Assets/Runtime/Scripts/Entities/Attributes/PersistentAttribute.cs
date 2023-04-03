using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    [CreateAssetMenu(fileName = "EntityAttribute", menuName = "Attributes/Attribute")]
    public class PersistentAttribute : ScriptableObject
    {
        [SerializeField]
        private AttributeType _type;
        [SerializeField]
        private float _maxValue;
        [SerializeField]
        private float _minValue;
        [SerializeField]
        private float _startingValue;

        public AttributeType Type => _type;
        public float MaxValue => _maxValue;
        public float MinValue => _minValue;
        public float StartingValue => _startingValue;
    }
}
