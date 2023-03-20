using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    [Serializable]
    public class StateTransition
    {
        [SerializeField]
        private StateType _fromState;
        [SerializeField]
        private Transition[] _transitions;

        public StateType FromState => _fromState;



        [Serializable]
        public struct Transition
        {
            public StateType state;
            public float probability;
        }
    } 
}
