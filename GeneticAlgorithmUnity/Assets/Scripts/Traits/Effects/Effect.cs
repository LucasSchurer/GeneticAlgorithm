using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Events;

namespace Game.Traits.Effects
{
    public abstract class Effect<Context> : ScriptableObject
where Context : EventContext
    {
        public abstract void Trigger(ref Context ctx, int currentStacks = 1);
    } 
}
