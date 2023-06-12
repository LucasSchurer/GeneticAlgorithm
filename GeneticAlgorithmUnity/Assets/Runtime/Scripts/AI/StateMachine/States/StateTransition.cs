using System;
using UnityEngine;

namespace Game.AI
{
    [Serializable]
    public class StateTransition<Actions>
    {
        [SerializeField]
        private Actions _action;
        [SerializeField]
        private StateProbability[] _states;

        public Actions Action => _action;

        [System.Serializable]
        private struct StateProbability
        {
            [SerializeField]
            private StateData _state;
            [SerializeField]
            private float _probability;

            public StateData State => _state;
            public float Probability => _probability;
        }

        public StateData Transition()
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            float k = 0f;

            for (int i = 0; i < _states.Length; i++)
            {
                k += _states[i].Probability;

                if (r <= k)
                {
                    return _states[i].State;
                }
            }

            return null;
        }
    } 
}
