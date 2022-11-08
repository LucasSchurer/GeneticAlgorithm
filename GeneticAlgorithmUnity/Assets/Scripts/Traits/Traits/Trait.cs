using UnityEngine;
using Game.Events;
using Game.Traits.Effects;

namespace Game.Traits
{
    public abstract class Trait<Type, Context> : ScriptableObject
        where Context : EventContext
    {
        public TraitIdentifier identifier;
        public TraitExecutionType executionType;
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
}
