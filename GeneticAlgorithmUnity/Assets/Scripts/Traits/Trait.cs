using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.ScriptableObjects
{
    public abstract class Trait<Type, Context> : ScriptableObject
        where Context: EventContext
    {
        public GameObject owner;
        public Type type;

        public virtual void Added(GameObject owner) => this.owner = owner;
        public abstract void Action(ref Context ctx);
        public virtual void Removed() { }
    } 
}
