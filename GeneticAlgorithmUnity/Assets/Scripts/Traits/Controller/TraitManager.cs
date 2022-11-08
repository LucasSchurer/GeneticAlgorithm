using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits
{
    public class TraitManager : MonoBehaviour
    {
        [SerializeField]
        private Traits<EntityEventType, EntityEventContext> _entityTraits;
        private Dictionary<TraitIdentifier, Trait<EntityEventType, EntityEventContext>> _entityTraitsDictionary;

        private static TraitManager _instance;

        public static TraitManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TraitManager>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                _entityTraitsDictionary = _entityTraits.BuildTraitsDictionary();
            }

            if (_instance != this)
            {
                Destroy(gameObject);
            }
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
    } 
}
