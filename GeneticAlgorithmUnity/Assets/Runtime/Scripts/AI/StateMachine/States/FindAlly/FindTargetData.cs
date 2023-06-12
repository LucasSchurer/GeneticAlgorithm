using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/FindAlly")]
    public class FindTargetData : StateData
    {
        public enum Action { FindTarget, FindSecondaryTarget };
        public enum ChoiceType { First, Random, Nearest, Furthest }

        [SerializeField]
        private LayerMask _targetLayerMask;
        [SerializeField]
        private LayerMask _secondaryTargetLayerMask;
        [SerializeField]
        private float _detectionRadius;
        [SerializeField]
        private ChoiceType _choiceType;
        [SerializeField]
        [Tooltip("If checked, the target will be the enemy's face instead of the body.")]
        private bool _tryToGetFace;
        [Tooltip("Actions not listed here will be ignored. Actions will be tested in the order provided")]
        [SerializeField]
        private Action[] _validActions;

        [SerializeField]
        private StateTransition<Action>[] _transitions;

        public LayerMask TargetLayerMask => _targetLayerMask;
        public float DetectionRadius => _detectionRadius;
        public ChoiceType Choice => _choiceType;
        public bool TryToGetFace => _tryToGetFace;
        public LayerMask SecondaryTargetLayerMask => _secondaryTargetLayerMask;
        public virtual Action[] ValidActions => _validActions;

        public override State GetState(StateMachine stateMachine)
        {
            return new FindTarget(stateMachine, this);
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