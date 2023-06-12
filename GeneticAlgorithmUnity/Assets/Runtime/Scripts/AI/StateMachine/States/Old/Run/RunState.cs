using UnityEngine;
using Game.Entities.Shared;

namespace Game.AI.States
{
    public class RunState : State
    {
        private RunStateData _data;

        private MovementController _movementController;
        private Transform _target;
        private Vector3 _direction;

        public RunState(StateMachine stateMachine, RunStateData data) : base(stateMachine, data)
        {
            _data = data;
            _movementController = stateMachine.GetComponent<MovementController>();
        }

        public override StateType GetStateType()
        {
            return StateType.Run;
        }

        public override void StateStart()
        {
            _target = GameObject.FindGameObjectWithTag(_data.TargetTag).transform;
        }

        public override void StateUpdate()
        {
            if (Vector3.Distance(_stateMachine.transform.position, _target.position) > _data.MaxRunDistance)
            {
                /*_stateMachine.ChangeCurrentState(StateType.Idle);*/
            }

            _direction = (_stateMachine.transform.position - _target.position).normalized;
        }

        public override void StateFixedUpdate()
        {
            _movementController.Rotate(_direction);
            _movementController.Move(_stateMachine.transform.forward);
        }

        public override void StateFinish()
        {

        }

        public override Vector3 GetLookDirection()
        {
            return Vector3.zero;
        }
    } 
}
