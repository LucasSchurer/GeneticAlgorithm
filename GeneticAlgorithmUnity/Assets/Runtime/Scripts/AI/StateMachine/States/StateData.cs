using UnityEngine;

namespace Game.AI.States
{
    public abstract class StateData : ScriptableObject
    {
        public abstract State GetState(StateMachine stateMachine);
        public abstract StateType GetStateType();

        protected StateData GetTransitionStateData<A>(A action, StateTransition<A>[] transitions)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                if (transitions[i].Action.Equals(action))
                {
                    return transitions[i].Transition();
                }
            }

            return null;
        }

        #region DEBUG

        /*public void BalanceProbabilities()
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
        }*/

        #endregion
    } 
}
