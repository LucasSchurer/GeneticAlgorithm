using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public abstract class Effect<Context> : ScriptableObject
where Context : EventContext
    {
        public virtual void WhenAdded(GameObject owner, int currentStacks) { }
        public virtual void Trigger(ref Context ctx, int currentStacks = 1) { }
        public virtual void Trigger(GameObject owner, int currentStacks = 1) { }
        public virtual void WhenRemoved(GameObject owner, int currentStacks) { }
    } 
}
