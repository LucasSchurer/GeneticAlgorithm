using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/FindAlly")]
    public class FindAllyData : StateData
    {
        public enum Action { FoundAlly };
        public enum ChoiceType { First, Random, Nearest, Furthest }

        [SerializeField]
        private LayerMask _allyLayerMask;
        [SerializeField]
        private float _detectionRadius;
        [SerializeField]
        private ChoiceType _choiceType;

        [SerializeField]
        private StateTransition<Action>[] _transitions;

        public LayerMask AllyLayerMask => _allyLayerMask;
        public float DetectionRadius => _detectionRadius;
        public ChoiceType Choice => _choiceType;

        public override State GetState(StateMachine stateMachine)
        {
            return new FindAlly(stateMachine, this);
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