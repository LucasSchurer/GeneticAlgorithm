using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    public class RunState : State
    {
        private Transform _target;
        private Vector3 _direction;
        private float _maxRunDistance = 15f;

        public RunState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override StateType GetStateType()
        {
            return StateType.Run;
        }

        public override void StateStart()
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void StateUpdate()
        {
            if (Vector3.Distance(_stateMachine.transform.position, _target.position) > _maxRunDistance)
            {
                _stateMachine.ChangeCurrentState(StateType.Idle);
            }

            _direction = (_stateMachine.transform.position - _target.position).normalized;
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
