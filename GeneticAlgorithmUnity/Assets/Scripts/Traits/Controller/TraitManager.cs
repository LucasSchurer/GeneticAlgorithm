using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System.Linq;

namespace Game.Traits
{
    public class TraitManager : Singleton<TraitManager>
    {
        [SerializeField]
        private Traits<EntityEventType, EntityEventContext> _entityTraits;
        private Dictionary<TraitIdentifier, Trait<EntityEventType, EntityEventContext>> _entityTraitsDictionary;
        private TraitIdentifier[] _traitIdentifiers;
        protected override void SingletonAwake()
        {
            _entityTraitsDictionary = _entityTraits.BuildTraitsDictionary();
            _traitIdentifiers = _entityTraitsDictionary.Keys.ToArray();
        }

        public Trait<EntityEventType, EntityEventContext> GetEntityTrait(TraitIdentifier identifier)
        {
            if (_entityTraitsDictionary != null && _entityTraitsDictionary.TryGetValue(identifier, out Trait<EntityEventType, EntityEventContext> trait))
            {
                return trait;
            } else
            {
                return null;
            }
        }

        public TraitIdentifier GetRandomTraitIdentifier()
        {
            return _traitIdentifiers[Random.Range(0, _traitIdentifiers.Length)];
        }
    } 
}
