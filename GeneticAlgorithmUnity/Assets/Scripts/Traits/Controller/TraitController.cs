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
        protected TraitTrigger _traitTrigger;

        protected class TraitTrigger
        {
            public TraitController<Type, Context, Controller> traitController;
            public Trait<Type, Context> trait;
            public bool canAct = true;

            public TraitTrigger(TraitController<Type, Context, Controller> traitController, Trait<Type, Context> trait)
            {
                this.traitController = traitController;
                this.trait = trait;
            }

            public void Trigger(ref Context ctx)
            {
                if (canAct)
                {
                    trait.TriggerEffects(ref ctx);
                    canAct = false;
                    traitController.StartCoroutine(CooldownCoroutine());
                }
            }

            private IEnumerator CooldownCoroutine()
            {
                yield return new WaitForSeconds(trait.cooldown);

                canAct = true;
            }
        }

        protected virtual void Awake()
        {
            _eventController = GetComponent<Controller>();
            _traitTrigger = new TraitTrigger(this, _trait);

            if (_eventController)
            {
            }
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController?.AddListener(_trait.eventType, _traitTrigger.Trigger, _trait.executionOrder);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController?.RemoveListener(_trait.eventType, _traitTrigger.Trigger, _trait.executionOrder);
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
