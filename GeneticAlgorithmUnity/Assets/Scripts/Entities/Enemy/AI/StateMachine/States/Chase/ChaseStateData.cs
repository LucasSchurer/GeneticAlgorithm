using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
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

        public string TargetTag => _targetTag;
        public float MaxChaseRange => _maxChaseRange;
        public float RequestPathTime => _requestPathTime;

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
