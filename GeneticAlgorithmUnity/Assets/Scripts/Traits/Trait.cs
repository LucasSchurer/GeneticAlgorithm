using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System;

public class Trait : MonoBehaviour, IEventListener
{
    public float cooldown;
    public EntityEventType eventType;
    public EventExecutionOrder executionOrder;
    public Effect<EntityEventContext>[] effects;
    public bool canAct = true;

    private void TriggerEffects(ref EntityEventContext ctx)
    {
        if (canAct)
        {
            foreach (Effect<EntityEventContext> effect in effects)
            {
                effect.Trigger(ref ctx);
            }
            
            canAct = false;
            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldown);

        canAct = true;
    }

    private void OnEnable()
    {
        StartListening();
    }

    private void OnDisable()
    {
        StopListening();
    }

    public void StartListening()
    {
        GetComponent<EntityEventController>()?.AddListener(eventType, TriggerEffects, executionOrder);
    }

    public void StopListening()
    {
        GetComponent<EntityEventController>()?.RemoveListener(eventType, TriggerEffects, executionOrder);
    }
}
