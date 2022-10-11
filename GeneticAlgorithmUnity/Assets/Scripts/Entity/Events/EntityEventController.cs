using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the events of a specific entity
/// Including method register/unregister and 
/// event invokes
/// </summary>
public class EntityEventController : MonoBehaviour
{
    public delegate void CombatEvent(EntityEventContext ctx);
    private Dictionary<EntityEventType, CombatEvent> _combatEvents;

    private void Awake()
    {
        if (_combatEvents == null)
        {
            _combatEvents = new Dictionary<EntityEventType, CombatEvent>();
        }
    }

    public void AddListener(EntityEventType type, CombatEvent callback)
    {
        if (_combatEvents == null)
        {
            _combatEvents = new Dictionary<EntityEventType, CombatEvent>();
        }

        if (_combatEvents.ContainsKey(type))
        {
            _combatEvents[type] += callback;
        }
        else
        {
            _combatEvents.Add(type, null);
            _combatEvents[type] += callback;
        }
    }

    public void RemoveListener(EntityEventType type, CombatEvent callback)
    {
        if (_combatEvents.ContainsKey(type))
        {
            _combatEvents[type] -= callback;
        }
    }

    public void EventTrigger(EntityEventType type, EntityEventContext ctx)
    {
        if (_combatEvents.ContainsKey(type))
        {
            _combatEvents[type]?.Invoke(ctx);
        }
    }
}
