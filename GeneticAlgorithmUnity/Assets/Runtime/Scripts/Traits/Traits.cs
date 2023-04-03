using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits
{
    public abstract class Traits<Type, Context> : ScriptableObject
        where Context: EventContext
    {
        [SerializeField]
        private Trait<Type, Context>[] _traits;

        public Dictionary<TraitIdentifier, Trait<Type, Context>> BuildTraitsDictionary()
        {
            Dictionary<TraitIdentifier, Trait<Type, Context>> dictionary = new Dictionary<TraitIdentifier, Trait<Type, Context>>();

            foreach (Trait<Type, Context> trait in _traits)
            {
                dictionary.TryAdd(trait.identifier, trait);
            }

            return dictionary;
        }
    } 
}
