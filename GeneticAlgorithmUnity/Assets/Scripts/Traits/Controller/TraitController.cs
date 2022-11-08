using UnityEngine;
using Game.Events;
using System.Collections.Generic;

namespace Game.Traits
{
    public class TraitController<Type, Context, Controller> : MonoBehaviour, IEventListener
        where Context : EventContext
        where Controller : EventController<Type, Context>
    {
        protected Controller _eventController;

        [SerializeField]
        protected Trait<Type, Context>[] _traits;
        protected List<TraitHandler<Type, Context, Controller>> _traitHandlers;

        protected virtual void Awake()
        {
            _eventController = GetComponent<Controller>();
            _traitHandlers = new List<TraitHandler<Type, Context, Controller>>(TraitHandler<Type, Context, Controller>.GetHandlersGivenTraits(this, _traits));
        }

        public void AddTrait(Trait<Type, Context> trait)
        {
            TraitHandler<Type, Context, Controller> traitHandler = new TraitHandler<Type, Context, Controller>(this, trait);
        }

        public void StartListening()
        {
            if (_eventController)
            {
                foreach (TraitHandler<Type, Context, Controller> handler in _traitHandlers)
                {
                    handler.StartListening(_eventController);
                }
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                foreach (TraitHandler<Type, Context, Controller> handler in _traitHandlers)
                {
                    handler.StopListening(_eventController);
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
