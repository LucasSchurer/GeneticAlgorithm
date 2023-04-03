using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    [CreateAssetMenu(fileName = "EntityAttributes", menuName = "Attributes/Attributes")]
    public class Attributes : ScriptableObject
    {
        [SerializeField]
        private PersistentAttribute[] _persistentAttributes;

        public Dictionary<AttributeType, NonPersistentAttribute> BuildNonPersistentAttributesDictionary()
        {
            Dictionary<AttributeType, NonPersistentAttribute> dictionary = new Dictionary<AttributeType, NonPersistentAttribute>();

            foreach (PersistentAttribute pAttribute in _persistentAttributes)
            {
                if (pAttribute.Type == AttributeType.None)
                {
                    continue;
                }


                dictionary.TryAdd(pAttribute.Type, new NonPersistentAttribute(pAttribute));
            }

            return dictionary;
        }
    } 
}
