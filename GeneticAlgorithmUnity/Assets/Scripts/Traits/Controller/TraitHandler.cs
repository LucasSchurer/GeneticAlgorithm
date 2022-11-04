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
        private bool canAct = true;

        public TraitHandler(TraitController<Type, Context, Controller> traitController, Trait<Type, Context> trait)
        {
            _traitController = traitController;
            _trait = trait;
        }

        public void Trigger(ref Context ctx)
        {
            if (canAct)
            {
                _trait.TriggerEffects(ref ctx);
                canAct = false;
                _traitController.StartCoroutine(CooldownCoroutine());
            }
        }

        public void StartListening(Controller eventController)
        {
            eventController.AddListener(_trait.eventType, Trigger, _trait.executionOrder);
        }

        public void StopListening(Controller eventController)
        {
            eventController.RemoveListener(_trait.eventType, Trigger, _trait.executionOrder);
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
