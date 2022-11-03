using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System;

public abstract class Trait<Type, Context> : ScriptableObject
    where Context: EventContext
{
    public TraitIdentifier identifier;
    public float cooldown;
    public Type eventType;
    public EventExecutionOrder executionOrder;
    public Effect<Context>[] effects;

    public void TriggerEffects(ref Context ctx)
    {
        foreach (Effect<Context> effect in effects)
        {
            effect.Trigger(ref ctx);
        }
    }
}
