using Game.Entities.Shared;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public class ShooterState : State
    {
        private ShooterStateData _data;
        private MovementController _movementController;
        private Transform _target;
        private Vector3 _lookDirection;
        private float _keepDistance;

        public ShooterState(StateMachine stateMachine, ShooterStateData data) : base(stateMachine, data)
        {
            _data = data;
            _stateMachine = stateMachine;
        }

        public override void StateFinish()
        {
            
        }

        public override void StateFixedUpdate()
        {
            if (_target)
            {
                Vector3 direction = (_target.position - _stateMachine.transform.position).normalized;

                if (Vector3.Distance(_target.position, _stateMachine.transform.position) <= _keepDistance)
                {
                    direction *= -1;
                }

                if (_stateMachine.CanMove() && _movementController != null)
                {
                    _movementController.Move(direction);
                }
            }
        }

        public override void StateStart()
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _keepDistance = Random.Range(_data.KeepDistanceRange.x, _data.KeepDistanceRange.y);
            _movementController = _stateMachine.GetComponent<MovementController>();
        }

        public override void StateUpdate()
        {
            if (_target)
            {
                _lookDirection = (_target.position - _stateMachine.transform.position).normalized;

                _stateMachine.EventController?.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, new EntityEventContext() { Movement = new EntityEventContext.MovementPacket() { LookDirection = _lookDirection }});
            }
        }

        public override Vector3 GetLookDirection()
        {
            return _lookDirection;
        }
    } 
}
