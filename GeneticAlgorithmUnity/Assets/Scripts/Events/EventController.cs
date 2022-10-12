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
                /*Event<Context> e = _events[type];
                e.AddListener(callback, order);*/
                _events[type]?.AddListener(callback, order);
            } else
            {
                _events.Add(type, new Event<Context>());
                _events[type]?.AddListener(callback, order);
            }
/*            if (_combatEvents == null)
            {
                _combatEvents = new Dictionary<EntityEventType, Event>();
            }

            if (_combatEvents.ContainsKey(type))
            {
                Event e = _combatEvents[type];
                e.AddListener(callback, executionOrder);
                _combatEvents[type] = e;
            }
            else
            {
                _combatEvents.Add(type, new Event());
                Event e = _combatEvents[type];
                e.AddListener(callback, executionOrder);
                _combatEvents[type] = e;
            }*/
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
                _events[type]?.Invoke(ctx);
            }
        }
    } 
}
