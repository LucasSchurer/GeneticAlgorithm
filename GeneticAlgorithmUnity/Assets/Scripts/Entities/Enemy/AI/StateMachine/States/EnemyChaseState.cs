using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    public class EnemyChaseState : EnemyState
    {
        private Transform _target;
        private Vector3 _direction;
        private float _maxChaseRange = 20f;

        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void StateStart()
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void StateUpdate()
        {
            if (Vector3.Distance(_stateMachine.transform.position, _target.position) > _maxChaseRange)
            {
                _stateMachine.ChangeCurrentState(EnemyStateType.Idle);
            }

            _direction = (_target.position - _stateMachine.transform.position).normalized;
        }

        public override void StateFixedUpdate()
        {
            _stateMachine.MovementController.Rotate(_direction);
            _stateMachine.MovementController.Move(_stateMachine.transform.forward);
        }

        public override void StateFinish()
        {
            
        }
    }
}
