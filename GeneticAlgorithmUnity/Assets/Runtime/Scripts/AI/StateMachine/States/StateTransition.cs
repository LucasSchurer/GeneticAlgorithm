using System;
using UnityEngine;

namespace Game.AI
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
