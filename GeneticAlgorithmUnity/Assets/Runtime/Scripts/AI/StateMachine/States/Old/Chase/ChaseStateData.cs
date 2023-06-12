using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(fileName = "ChaseStateData", menuName = "AI/StateMachine/States/ChaseStateData")]
    public class ChaseStateData : StateData
    {
        [SerializeField]
        private string _targetTag;
        [SerializeField]
        private float _maxChaseRange = 20f;
        [SerializeField]
        private float _requestPathTime = 1f;
        [SerializeField]
        private AvoidanceData _avoidanceData;

        public string TargetTag => _targetTag;
        public float MaxChaseRange => _maxChaseRange;
        public float RequestPathTime => _requestPathTime;
        public AvoidanceData AvoidanceData => _avoidanceData;

        public override State GetState(StateMachine stateMachine)
        {
            return new ChaseState(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.Chase;
        }
    }
}
