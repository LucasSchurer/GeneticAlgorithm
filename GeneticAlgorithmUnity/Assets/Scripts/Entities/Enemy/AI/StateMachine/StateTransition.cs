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

        public StateType GetTransition()
        {
            float p = UnityEngine.Random.Range(0f, 1f);
            float pSum = 0f;

            foreach (Transition t in _transitions)
            {
                pSum += t.probability;

                if (p <= pSum)
                {
                    return t.state;
                }
            }

            return StateType.Idle;
        }

        [Serializable]
        public struct Transition
        {
            public StateType state;
            public float probability;
        }
    } 
}
