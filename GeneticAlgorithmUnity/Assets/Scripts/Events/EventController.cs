using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public abstract class EventController<Type, Context> : MonoBehaviour
        where Context: EventContext
    {
        protected Dictionary<Type, Event<Context>> _events;

        protected virtual void Awake()
        {
            if (_events == null)
            {
                _events = new Dictionary<Type, Event<Context>>();
            }
        }

        public virtual void AddListener(Type type, Event<Context>.Delegate callback, EventExecutionOrder order = EventExecutionOrder.Standard)
        {
            if (_events == null)
            {
                _events = new Dictionary<Type, Event<Context>>();
            }

            if (_events.ContainsKey(type))
            {
                _events[type]?.AddListener(callback, order);
            } else
            {
                _events.Add(type, new Event<Context>());
                _events[type]?.AddListener(callback, order);
            }
        }

        public virtual void RemoveListener(Type type, Event<Context>.Delegate callback, EventExecutionOrder order = EventExecutionOrder.Standard)
        {
            if (_events.ContainsKey(type))
            {
                _events[type]?.RemoveListener(callback, order);
            }
        }

        public virtual void TriggerEvent(Type type, Context ctx)
        {
            if (_events.ContainsKey(type))
            {
                AddEventControllerToContext(ref ctx);
                ctx.owner = gameObject;
                _events[type]?.Invoke(ctx);
            }
        }

        protected abstract void AddEventControllerToContext(ref Context ctx);
    } 
}
