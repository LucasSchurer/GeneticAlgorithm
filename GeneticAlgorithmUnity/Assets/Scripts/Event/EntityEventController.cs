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
    public delegate void CombatEvent(CombatEventContext ctx);
    private Dictionary<CombatEventType, CombatEvent> _combatEvents;

    private void Awake()
    {
        if (_combatEvents == null)
        {
            _combatEvents = new Dictionary<CombatEventType, CombatEvent>();
        }
    }

    public void AddListener(CombatEventType type, CombatEvent callback)
    {
        if (_combatEvents == null)
        {
            _combatEvents = new Dictionary<CombatEventType, CombatEvent>();
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

    public void RemoveListener(CombatEventType type, CombatEvent callback)
    {
        if (_combatEvents.ContainsKey(type))
        {
            _combatEvents[type] -= callback;
        }
    }

    public void EventTrigger(CombatEventType type, CombatEventContext ctx)
    {
        if (_combatEvents.ContainsKey(type))
        {
            _combatEvents[type]?.Invoke(ctx);
        }
    }
}
