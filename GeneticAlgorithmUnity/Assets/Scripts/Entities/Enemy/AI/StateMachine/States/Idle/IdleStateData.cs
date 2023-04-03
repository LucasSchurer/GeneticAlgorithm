using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    [CreateAssetMenu(fileName = "IdleStateData", menuName = "AI/StateMachine/States/IdleStateData")]
    public class IdleStateData : StateData
    {
        [Header("Settings")]
        [SerializeField]
        private Vector2 _turnTimeRange = new Vector2(1, 4);
        [SerializeField]
        private float _detectionRange = 10f;
        [SerializeField]
        private LayerMask _detectionLayer;

        public Vector2 TurnTimeRange => _turnTimeRange;
        public float DetectionRange => _detectionRange;
        public LayerMask DetectionLayer => _detectionLayer;

        public override StateType GetStateType()
        {
            return StateType.Idle;
        }

        public override State GetState(StateMachine stateMachine)
        {
            return new IdleState(stateMachine, this);
        }
    }
}
