using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public interface IEventListener
    {
        public void StartListening();
        public void StopListening();
    } 
}

