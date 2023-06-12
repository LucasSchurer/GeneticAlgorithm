using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/SupportDefault")]
    public class SupportDefaultData : StateData
    {
        public enum Action { Start, CloseToDeath }        

        [SerializeField]
        private StateTransition<Action>[] _transitions;

        public override State GetState(StateMachine stateMachine)
        {
            return new SupportDefault(stateMachine, this);
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