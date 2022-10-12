using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the events of a specific entity
/// Including method register/unregister and 
/// event invokes
/// </summary>
namespace Game.Events
{
    public class EntityEventController : MonoBehaviour
    {
        public delegate void CombatEvent(ref EntityEventContext ctx);
        private Dictionary<EntityEventType, Event> _combatEvents;
        private Dictionary<EntityEventType, Event<EntityEventContext>> _entityEvents;

        private struct Event
        {
            public CombatEvent before;
            public CombatEvent standard;
            public CombatEvent after;

            public void AddListener(CombatEvent callback, EventExecutionOrder order)
            {
                switch (order)
                {
                    case EventExecutionOrder.Before:
                        before += callback;
                        break;
                    case EventExecutionOrder.Standard:
                        standard += callback;
                        break;
                    case EventExecutionOrder.After:
                        after += callback;
                        break;
                    default:
                        standard += callback;
                        break;
                }
            }

            public void Invoke(EntityEventContext ctx)
            {
                before?.Invoke(ref ctx);
                standard?.Invoke(ref ctx);
                after?.Invoke(ref ctx);
            }
        }

        private void Awake()
        {
            if (_combatEvents == null)
            {
                _combatEvents = new Dictionary<EntityEventType, Event>();
            }

            if (_entityEvents == null)
            {
                _entityEvents = new Dictionary<EntityEventType, Event<EntityEventContext>>();
            }
        }

        public void AddListener(EntityEventType type, CombatEvent callback, EventExecutionOrder executionOrder = EventExecutionOrder.Standard)
        {
            if (_combatEvents == null)
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
            }
        }

        public void RemoveListener(EntityEventType type, CombatEvent callback)
        {
            if (_combatEvents.ContainsKey(type))
            {
                /*_combatEvents[type] -= callback;*/
            }
        }

        public void EventTrigger(EntityEventType type, EntityEventContext ctx)
        {
            if (_combatEvents.ContainsKey(type))
            {
                _combatEvents[type].Invoke(ctx);
            }
        }
    } 
}
