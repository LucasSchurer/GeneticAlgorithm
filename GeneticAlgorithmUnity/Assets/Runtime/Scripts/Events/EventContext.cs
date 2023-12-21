using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public abstract class EventContext
    {
        // The owner of the eventController
        protected GameObject _owner;
        // The other object (if exists) related to the event
        protected GameObject _other;        

        public GameObject Owner { get => _owner; set => _owner = value; }
        public GameObject Other { get => _other; set => _other = value; }
    } 
}
