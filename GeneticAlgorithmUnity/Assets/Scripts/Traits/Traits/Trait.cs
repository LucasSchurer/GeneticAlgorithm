using UnityEngine;
using Game.Events;
using Game.Traits.Effects;

namespace Game.Traits
{
    public abstract class Trait<Type, Context> : ScriptableObject
        where Context : EventContext
    {
        public TraitIdentifier identifier = TraitIdentifier.None;
        public TraitExecutionType executionType;
        public float cooldown;
        public Type eventType;
        public EventExecutionOrder executionOrder = EventExecutionOrder.Standard;
        public Effect<Context>[] effects;
        [Tooltip("Max stacks allowed for the trait. 1 = only one copy of the trait is possible")]
        [Range(1, 10)]
        public int maxStacks = 1;

        public void TriggerEffects(ref Context ctx, int currentStacks = 1)
        {
            foreach (Effect<Context> effect in effects)
            {
                effect.Trigger(ref ctx, currentStacks);
            }
        }
    } 
}
