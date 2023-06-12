using System.Collections;
using UnityEngine;
using Game.Entities.Shared;

namespace Game.AI.States
{
    public class IdleState : State
    {
        private IdleStateData _data;
        private MovementController _movementController;
        private Coroutine _idleCoroutine;

        private Vector3 _direction = new Vector3(1, 0, 1);

        public IdleState(StateMachine stateMachine, IdleStateData data) : base(stateMachine, data)
        {
            _data = data;
            _movementController = _stateMachine.GetComponent<MovementController>();
        }

        public override StateType GetStateType()
        {
            return StateType.Idle;
        }

        public override void StateStart()
        {
            _idleCoroutine = _stateMachine.StartCoroutine(IdleCoroutine());
        }

        public override void StateUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(_stateMachine.transform.position, _data.DetectionRange, _data.DetectionLayer);

            if (colliders.Length > 0)
            {
                /*_stateMachine.ChangeCurrentState(_data.GetTransitionState());*/
            }

            Debug.DrawRay(_stateMachine.transform.position, _direction * 2f, Color.blue);
        }

        public override void StateFixedUpdate()
        {
            if (_movementController)
            {
                try
                {
                    /*_movementController.Rotate(_direction);*/
                    _movementController.Move(_direction);
                } catch (System.NullReferenceException e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public override void StateFinish()
        {
            if (_idleCoroutine != null)
            {
                _stateMachine.StopCoroutine(_idleCoroutine);
            }
        }

        private IEnumerator IdleCoroutine()
        {
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

            _direction = direction;

            yield return new WaitForSeconds(Random.Range(_data.TurnTimeRange.x, _data.TurnTimeRange.y));

            _idleCoroutine = _stateMachine.StartCoroutine(IdleCoroutine());
        }

        public override Vector3 GetLookDirection()
        {
            return Vector3.zero;
        }
    }
}
