using System.Collections;
using UnityEngine;
using Game.Events;

namespace Game.Traits
{
    public class TraitController<Type, Context, Controller> : MonoBehaviour, IEventListener
        where Context : EventContext
        where Controller : EventController<Type, Context>
    {
        protected Controller _eventController;

        [SerializeField]
        protected Trait<Type, Context>[] _traits;
        protected TraitTrigger[] _traitTriggers;

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
            _traitTriggers = new TraitTrigger[_traits.Length];

            if (_eventController)
            {
                for (int i = 0; i < _traits.Length; i++)
                {
                    _traitTriggers[i] = new TraitTrigger(this, _traits[i]);
                }
            }
        }

        public void StartListening()
        {
            if (_eventController)
            {
                foreach (TraitTrigger trigger in _traitTriggers)
                {
                    _eventController.AddListener(trigger.trait.eventType, trigger.Trigger, trigger.trait.executionOrder);
                }
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                foreach (TraitTrigger trigger in _traitTriggers)
                {
                    _eventController.RemoveListener(trigger.trait.eventType, trigger.Trigger, trigger.trait.executionOrder);
                }
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
