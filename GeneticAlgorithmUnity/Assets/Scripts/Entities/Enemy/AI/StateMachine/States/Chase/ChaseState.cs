using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    public class ChaseState : State
    {
        private ChaseStateData _data;
        private MovementController _movementController;

        private Transform _target;
        private Vector3 _direction;

        public ChaseState(StateMachine stateMachine, ChaseStateData data) : base(stateMachine, data)
        {
            _data = data;
            _movementController = stateMachine.GetComponent<MovementController>();
        }

        public override StateType GetStateType()
        {
            return StateType.Chase;
        }

        public override void StateStart()
        {
            _target = GameObject.FindGameObjectWithTag(_data.TargetTag).transform;
        }

        public override void StateUpdate()
        {
            if (Vector3.Distance(_stateMachine.transform.position, _target.position) > _data.MaxChaseRange)
            {
                _stateMachine.ChangeCurrentState(StateType.Idle);
            }

            _direction = (_target.position - _stateMachine.transform.position).normalized;
        }

        public override void StateFixedUpdate()
        {
            _movementController.Rotate(_direction);
            _movementController.Move(_stateMachine.transform.forward);
        }

        public override void StateFinish()
        {
            
        }
    }
}
