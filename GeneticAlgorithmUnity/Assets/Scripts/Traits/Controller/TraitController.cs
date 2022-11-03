using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.ScriptableObjects;
using System;

namespace Game
{
    public class TraitController<Type, Context, Controller> : MonoBehaviour, IEventListener
        where Context : EventContext
        where Controller : EventController<Type, Context>
    {
        protected Controller _eventController;

        [SerializeField]
        protected Trait<Type, Context> _trait;

        protected virtual void Awake()
        {
            _eventController = GetComponent<Controller>();

            if (_eventController)
            {
            }
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController?.AddListener(_trait.eventType, _trait.TriggerEffects, _trait.executionOrder);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController?.RemoveListener(_trait.eventType, _trait.TriggerEffects, _trait.executionOrder);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }
    }
}
