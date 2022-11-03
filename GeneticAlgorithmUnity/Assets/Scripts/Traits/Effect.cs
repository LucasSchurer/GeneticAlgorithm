using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Events;

public abstract class Effect<Context> : ScriptableObject
    where Context : EventContext
{
    public abstract void Trigger(ref Context ctx); 
}
