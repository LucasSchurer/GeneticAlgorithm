using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.ScriptableObjects;
using System;

namespace Game.Entities
{
    public class EntityTraits : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private List<Trait<EntityEventType, EntityEventContext>> _traits;
        [SerializeField]
        private EntityEventController _eventController;


        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();

            if (_eventController)
            {
                foreach (Trait<EntityEventType, EntityEventContext> trait in _traits)
                {
                    trait.Added(gameObject);
                    _eventController.AddListener(trait.type, trait.Action);
                }
            }

        }


        public void StartListening()
        {
            throw new System.NotImplementedException();
        }

        public void StopListening()
        {
            throw new System.NotImplementedException();
        }

        private void OnDestroy()
        {
            foreach (Trait<EntityEventType, EntityEventContext> trait in _traits)
            {
                trait.Added(gameObject);
            }
        }
    } 
}
