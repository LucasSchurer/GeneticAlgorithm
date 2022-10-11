using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;

namespace Game.Events
{
    public class EntityEventContext
    {
        public Entity owner;
        public Entity target;
        public float healthModifier;
    } 
}
