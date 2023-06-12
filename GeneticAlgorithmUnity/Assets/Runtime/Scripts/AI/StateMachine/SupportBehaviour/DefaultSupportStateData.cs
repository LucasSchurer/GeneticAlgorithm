using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/DefaultSupportStateData")]
    public class DefaultSupportStateData : StateData
    {
        public enum Action { Start, CloseToDeath }        

        [SerializeField]
        private StateTransition<Action>[] _transitions;

        public override State GetState(StateMachine stateMachine)
        {
            return new DefaultSupportState(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }

        public State GetTransitionState(StateMachine stateMachine, Action action)
        {
            StateData stateData = GetTransitionStateData(action, _transitions);

            if (stateData == null)
            {
                return null;
            }

            return stateData.GetState(stateMachine);
        }
    } 
}