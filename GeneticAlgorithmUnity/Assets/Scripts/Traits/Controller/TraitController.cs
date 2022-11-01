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
        protected Trait _trait;
        protected List<TraitAction> _actions;

        protected virtual void Awake()
        {
            _eventController = GetComponent<Controller>();
            _actions = new List<TraitAction>();

            if (_eventController)
            {
                /*foreach (TraitSO<Type, Context, Trait<Type, Context>> trait in _traitsSO)
                {
                    trait.Added(gameObject);
                    _eventController.AddListener(trait.type, trait.Action);
                }*/
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
            /*foreach (TraitSO<Type, Context, Trait<Type, Context>> trait in _traitsSO)
            {
                trait.Added(gameObject);
            }*/
        }
    }
}
