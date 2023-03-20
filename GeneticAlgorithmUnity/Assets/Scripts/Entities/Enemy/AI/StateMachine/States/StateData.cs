using UnityEngine;

namespace Game.Entities.AI
{
    public abstract class StateData : ScriptableObject
    {
        [SerializeField]
        protected StateTransition[] _transitions;
        public abstract State GetState(StateMachine stateMachine);
        public abstract StateType GetStateType();

        public StateType GetTransitionState()
        {
            float p = Random.Range(0f, 1f);
            float pSum = 0f;

            foreach (StateTransition t in _transitions)
            {
                pSum += t.probability;

                if (p <= pSum)
                {
                    return t.state;
                }
            }

            return StateType.None;
        }

        [System.Serializable]
        public struct StateTransition
        {
            public StateType state;
            [Range(0f, 1f)]
            public float probability;
        }

        #region DEBUG

        public void BalanceProbabilities()
        {
            float probabilitySum = GetProbabilitiesSum();

            for (int i = 0; i < _transitions.Length; i++)
            {
                if (_transitions.Length == 1)
                {
                    _transitions[i].probability = 1f;
                    break;
                }

                _transitions[i].probability /= probabilitySum;
            }
        }

        public float GetProbabilitiesSum()
        {
            float sum = 0f;

            foreach (StateTransition t in _transitions)
            {
                sum += t.probability;
            }

            return sum;
        }

        public bool HasRepeatedTransition()
        {
            for (int i = 0; i < _transitions.Length; i++)
            {
                for (int j = 0; j < _transitions.Length; j++)
                {
                    if (i == j) continue;

                    if (_transitions[i].state == _transitions[j].state)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    } 
}
