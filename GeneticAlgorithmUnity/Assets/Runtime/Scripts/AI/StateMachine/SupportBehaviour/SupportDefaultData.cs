using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/SupportDefault")]
    public class SupportDefaultData : StateData
    {
        public enum Action { Start, CloseToDeath }

        [SerializeField]
        [Range(0, 1)]
        private float _closeToDeathHealthThreshold = 0f;

        [SerializeField]
        private StateTransition<Action>[] _transitions;

        public float CloseToDeathHealthThreshold => _closeToDeathHealthThreshold;

        public override State GetState(StateMachine stateMachine)
        {
            return new SupportDefault(stateMachine, this);
        }

        public State GetTransitionState(StateMachine stateMachine, HashSet<Action> blockedActions, Action action)
        {
            StateData stateData = GetTransitionStateData(action, blockedActions, _transitions);

            if (stateData == null)
            {
                return null;
            }

            return stateData.GetState(stateMachine);
        }
    } 
}