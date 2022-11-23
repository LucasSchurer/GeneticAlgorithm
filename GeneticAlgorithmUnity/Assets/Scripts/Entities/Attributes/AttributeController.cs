using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public class AttributeController : MonoBehaviour
    {
        [SerializeField]
        private Attributes _attributes;
        private Dictionary<AttributeType, NonPersistentAttribute> _npAttributesDictionary;

        private void Awake()
        {
            _npAttributesDictionary = _attributes.BuildNonPersistentAttributesDictionary();
        }

        public void ModifyNonPersistentAttributeCurrentValue(AttributeType type, float amount)
        {
            if (_npAttributesDictionary.TryGetValue(type, out NonPersistentAttribute npAttribute))
            {
                npAttribute.CurrentValue = npAttribute.CurrentValue + amount;
            }
        }

        public NonPersistentAttribute GetNonPersistentAttribute(AttributeType type)
        {
            if (_npAttributesDictionary.TryGetValue(type, out NonPersistentAttribute npAttribute))
            {
                return npAttribute;
            } else
            {
                return new NonPersistentAttribute();
            }
        }
    } 
}
