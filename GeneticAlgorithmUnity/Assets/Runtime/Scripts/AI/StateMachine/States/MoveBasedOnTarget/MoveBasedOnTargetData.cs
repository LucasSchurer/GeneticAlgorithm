using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/MoveBasedOnTarget")]
    public class MoveBasedOnTargetData : StateData
    {
        public enum Action { TargetDead, TargetCloseToDeath, CloseToDeath };

        [Header("General Settings")]
        [SerializeField]
        private Vector3 _minPositionOffset;
        [SerializeField]
        private Vector3 _maxPositionOffset;
        [SerializeField]
        [Tooltip("If checked, the offset will consider the target view direction. Example: a position of 0, 0, -1 will ALWAYS be at the target's back.")]
        private bool _useTargetViewDirection;
        [SerializeField]
        [Tooltip("If checked, will randomize the offset after a certain amount of time.")]
        private bool _randomizeOffsetMultipleTimes;
        [SerializeField]
        private float _randomizeOffsetInterval;
        [SerializeField]
        private float _minimumDistanceToNewPosition;
        [SerializeField]
        [Tooltip("If set to > 0 will update the position inside a coroutine. If set to 0 will update every frame.")]
        private float _newPositionInterval;
        [SerializeField]
        [Tooltip("If set to > 0 will update the position inside a coroutine. If set to 0 will update every frame.")]
        private float _checkActionsInterval;
        [SerializeField]
        private float _radiusToStopMoving;
        [SerializeField]
        private float _radiusToStartMoving;
        [SerializeField]
        private bool _canFire = true;
        [SerializeField]
        private bool _lookToPlayer = true;
        public enum FacingType { Target, Ally, Enemy }
        [SerializeField]
        private FacingType _facingType;
        public FacingType Facing => _facingType;
        [SerializeField]
        private float _newTargetDetectionTime = 1f;
        public float NewTargetDetectionTime => _newTargetDetectionTime;

        [Header("Action Related Settings")]
        [SerializeField]
        [Range(0, 1)]
        private float _closeToDeathHealthThreshold = 0f;
        [SerializeField]
        [Tooltip("If checked, will check if health > threshold.")]
        private bool _closeToDeathInverse = false;
        [SerializeField]
        [Range(0, 1)]
        private float _targetCloseToDeathThreshold = 0f;
        [Tooltip("Actions not listed here will be ignored. Actions will be tested in the order provided")]
        [SerializeField]
        private Action[] _validActions;

        public float CloseToDeathHealthThreshold => _closeToDeathHealthThreshold;
        public float TargetCloseToDeathThreshold => _targetCloseToDeathThreshold;
        public bool CloseToDeathInverse => _closeToDeathInverse;
        public Vector3 MinPositionOffset => _minPositionOffset;
        public Vector3 MaxPositionOffset => _maxPositionOffset;
        public bool UseTargetViewDirection => _useTargetViewDirection;
        public bool RandomizeOffsetMultipleTimes => _randomizeOffsetMultipleTimes;
        public float RandomizeOffsetInterval => _randomizeOffsetInterval;
        public float MinimumDistanceToNewPosition => _minimumDistanceToNewPosition;
        public float NewPositionInterval => _newPositionInterval;
        public float CheckActionsInterval => _checkActionsInterval;
        public virtual Action[] ValidActions => _validActions;
        public float RadiusToStopMoving => _radiusToStopMoving;
        public float RadiusToStartMoving => _radiusToStartMoving;
        public bool CanFire => _canFire;
        public bool LookToPlayer => _lookToPlayer;

        [SerializeField]
        private StateTransition<Action>[] _transitions;

        public override State GetState(StateMachine stateMachine)
        {
            return new MoveBasedOnTarget(stateMachine, this);
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