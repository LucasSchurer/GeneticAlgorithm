using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;

namespace Game.Events
{
    public class EntityEventContext : EventContext
    {
        public GameObject target;
        public EntityEventController eventController;
        public float healthModifier;
    } 
}
