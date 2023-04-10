using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public abstract class EventContext
    {
        protected GameObject _owner;
        protected GameObject _other;

        public GameObject Owner { get => _owner; set => _owner = value; }
        public GameObject Other { get => _other; set => _other = value; }
    } 
}
