using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(fileName = "StateMachineData", menuName = "AI/StateMachine/StateMachineData")]
    public class StateMachineData : ScriptableObject
    {
        [SerializeField]
        private StateData _initialState;

        public State GetInitialState(StateMachine stateMachine)
        {
            return _initialState.GetState(stateMachine);
        }

/*        public Dictionary<StateType, State> GetStates(StateMachine stateMachine)
        {
            Dictionary<StateType, State> states = new Dictionary<StateType, State>();

            foreach (StateData stateData in _states)
            {
                states.TryAdd(stateData.GetStateType(), stateData.GetState(stateMachine));
            }

            return states;
        }*/
    } 
}
