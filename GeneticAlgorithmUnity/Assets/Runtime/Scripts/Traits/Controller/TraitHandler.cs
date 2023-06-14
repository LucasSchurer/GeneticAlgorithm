using System.Collections;
using UnityEngine;
using Game.Events;

namespace Game.Traits
{
    public class TraitHandler<Type, Context, Controller>
        where Context: EventContext
        where Controller: EventController<Type, Context>
    {
        private TraitController<Type, Context, Controller> _traitController;
        private Trait<Type, Context> _trait;
        private int _currentStacks = 1;
        private bool canAct = true;
        private bool isListeningToEvent = false;

        public int Stacks => _currentStacks;
        public Trait<Type, Context> Trait => _trait;

        public TraitIdentifier GetTraitIdentifier => _trait.identifier;

        public TraitHandler(TraitController<Type, Context, Controller> traitController, Trait<Type, Context> trait)
        {
            _traitController = traitController;
            _trait = trait;
        }

        public void WhenAdded()
        {
            _trait.WhenAdded(_traitController.gameObject, _currentStacks);
        }

        public void WhenRemoved()
        {
            _trait.WhenRemoved(_traitController.gameObject, _currentStacks);
        }

        public bool TryAddStack()
        {
            if (_currentStacks < _trait.maxStacks)
            {
                _currentStacks++;

                return true;
            }

            return false;
        }

        public void Trigger(ref Context ctx)
        {
            if (canAct)
            {
                if (_trait.executionType == TraitExecutionType.EventBased)
                {
                    _trait.TriggerEffects(ref ctx, _currentStacks);

                    if (_trait.cooldown > 0)
                    {
                        canAct = false;
                        _traitController.StartCoroutine(CooldownCoroutine());
                    }
                }
            }
        }

        public void Trigger()
        {
            if (canAct)
            {
                _trait.TriggerEffects(_traitController.gameObject);

                if (_trait.cooldown > 0)
                {
                    canAct = false;
                    _traitController.StartCoroutine(CooldownCoroutine());
                }
            }
        }

        public void TriggerConstant()
        {
            _trait.TriggerEffects(_traitController.gameObject, _currentStacks);
        }

        public void StartListening(Controller eventController)
        {
            if (!isListeningToEvent && _trait.executionType == TraitExecutionType.EventBased)
            {
                eventController.AddListener(_trait.eventType, Trigger, _trait.executionOrder);
                isListeningToEvent = true;
            }
        }

        public void StopListening(Controller eventController)
        {
            if (isListeningToEvent && _trait.executionType == TraitExecutionType.EventBased)
            {
                eventController.RemoveListener(_trait.eventType, Trigger, _trait.executionOrder);
                isListeningToEvent = false;
            }
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(_trait.cooldown);

            canAct = true;
        }

        public static TraitHandler<Type, Context, Controller>[] GetHandlersGivenTraits(TraitController<Type, Context, Controller> traitController, Trait<Type, Context>[] traits)
        {
            TraitHandler<Type, Context, Controller>[] handlers = new TraitHandler<Type, Context, Controller>[traits.Length];

            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i] = new TraitHandler<Type, Context, Controller>(traitController, traits[i]);
            }

            return handlers;
        }
    }
}
