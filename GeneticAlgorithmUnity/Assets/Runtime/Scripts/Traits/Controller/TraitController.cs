using UnityEngine;
using Game.Events;
using System.Collections.Generic;
using System.Linq;

namespace Game.Traits
{
    public abstract class TraitController<Type, Context, Controller> : MonoBehaviour, IEventListener
        where Context : EventContext
        where Controller : EventController<Type, Context>
    {
        protected Controller _eventController;

        [SerializeField]
        protected Trait<Type, Context>[] _traits;
        protected Dictionary<TraitIdentifier, TraitHandler<Type, Context, Controller>> _traitHandlers;
        protected HashSet<TraitHandler<Type, Context, Controller>> _constantTraits;

        protected virtual void Awake()
        {
            _eventController = GetComponent<Controller>();
            _traitHandlers = new Dictionary<TraitIdentifier, TraitHandler<Type, Context, Controller>>();
            _constantTraits = new HashSet<TraitHandler<Type, Context, Controller>>();
        }

        protected virtual void Start()
        {
            foreach (Trait<Type, Context> trait in _traits)
            {
                AddTrait(trait);
            }
        }
        
        protected virtual void Update()
        {
            foreach (TraitHandler<Type, Context, Controller> handler in _constantTraits)
            {
                handler.TriggerConstant();
            }
        }

        public void AddTrait(Trait<Type, Context> trait)
        {
            if (_traitHandlers.TryGetValue(trait.identifier, out TraitHandler<Type, Context, Controller> handler))
            {
                if (handler.TryAddStack() && trait.executionType == TraitExecutionType.WhenAdded)
                {
                    Context ctx = GetContextForWhenAddedTraits();
                    handler.Trigger(ref ctx);
                }
            } else
            {
                TraitHandler<Type, Context, Controller> traitHandler = new TraitHandler<Type, Context, Controller>(this, trait);

                switch (trait.executionType)
                {
                    case TraitExecutionType.EventBased:
                        traitHandler.StartListening(_eventController);
                        break;
                    case TraitExecutionType.WhenAdded:
                        Context ctx = GetContextForWhenAddedTraits();
                        traitHandler.Trigger(ref ctx);
                        break;
                    case TraitExecutionType.Constant:
                        _constantTraits.Add(traitHandler);
                        break;
                }

                _traitHandlers.Add(trait.identifier, traitHandler);
            }
        }

        public void RemoveTrait(Trait<Type, Context> trait)
        {
            if (_traitHandlers.TryGetValue(trait.identifier, out TraitHandler<Type, Context, Controller> handler))
            {
                handler.StopListening(_eventController);

                _traitHandlers.Remove(trait.identifier);

                if (trait.executionType == TraitExecutionType.Constant)
                {
                    _constantTraits.Remove(handler);
                }
            }
        }

        protected abstract Context GetContextForWhenAddedTraits();

        public TraitIdentifier[] GetTraitsIdentifiers()
        {
            return _traitHandlers.Keys.ToArray();
        }

        public void StartListening()
        {
            if (_eventController)
            {
                foreach (TraitHandler<Type, Context, Controller> handler in _traitHandlers.Values)
                {
                    handler.StartListening(_eventController);
                }
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                foreach (TraitHandler<Type, Context, Controller> handler in _traitHandlers.Values)
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
