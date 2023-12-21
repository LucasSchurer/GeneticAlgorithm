using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/FindAlly")]
    public class FindTargetData : StateData
    {
        public enum Action { FindTarget, FindSecondaryTarget };
        public enum ChoiceType { First, Random, Nearest, Furthest }

        public enum TargetType { AllAllies, AllyCreature, AllyShield, AllEnemies, EnemyCreature, EnemyShield, AllCreature, AllShield }

        [SerializeField]
        private TargetType _target;
        [SerializeField]
        private TargetType _secondaryTarget;
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

        [SerializeField]
        private float _targetFindRetryTime = 1f;
        [SerializeField]
        private int _maximumRetryCount = 5;

        public float TargetFindRetryTime => _targetFindRetryTime;
        public int MaximumRetryCount => _maximumRetryCount;

        public float DetectionRadius => _detectionRadius;
        public ChoiceType Choice => _choiceType;
        public bool TryToGetFace => _tryToGetFace;
        public virtual Action[] ValidActions => _validActions;
        public TargetType Target => _target;
        public TargetType SecondaryTarget => _secondaryTarget;

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

        public LayerMask GetTargetLayerMask(TargetType target, Events.Entity entity)
        {
            switch (target)
            {
                case TargetType.AllAllies:
                    return entity.AllyLayer | entity.AllyShieldLayer;
                case TargetType.AllyCreature:
                    return entity.AllyLayer;
                case TargetType.AllyShield:
                    return entity.AllyShieldLayer;
                case TargetType.AllEnemies:
                    return entity.EnemyLayer | entity.EnemyShieldLayer;
                case TargetType.EnemyCreature:
                    return entity.EnemyLayer;
                case TargetType.EnemyShield:
                    return entity.EnemyShieldLayer;
                case TargetType.AllCreature:
                    return entity.AllyLayer | entity.EnemyLayer;
                case TargetType.AllShield:
                    return entity.AllyShieldLayer | entity.EnemyShieldLayer;
                default:
                    return entity.AllyLayer;
            }
        }
    }
}