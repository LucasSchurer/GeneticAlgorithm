using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public abstract class EventContext
    {
        public GameObject owner;
        public GameObject agent;
        public GameObject other;
    } 
}
