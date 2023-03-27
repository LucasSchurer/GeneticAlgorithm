using System.Collections;
using UnityEngine;

namespace Game.Entities.AI
{
    public class ChaseState : State
    {
        private ChaseStateData _data;
        private MovementController _movementController;

        private Transform _target;
        private Vector3 _direction;

        private Coroutine _requestPathCoroutine;

        private Vector3[] _steps;
        private int _currentStep;

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
            if (_target == null)
            {
                _target = GameObject.FindGameObjectWithTag(_data.TargetTag).transform;
            }

            _requestPathCoroutine = _stateMachine.StartCoroutine(RequestPathCoroutine());
        }

        public override void StateUpdate()
        {
            // Transition condition
            if (Vector3.Distance(_stateMachine.transform.position, _target.position) > _data.MaxChaseRange)
            {
                _stateMachine.ChangeCurrentState(StateType.Idle);
                return;
            }

            UpdateDirection();
            DrawPath();
        }

        private void UpdateDirection()
        {
            if (_steps != null && _currentStep >= 0 && _currentStep < _steps.Length)
            {
                if (Vector3.Distance(_stateMachine.transform.position, _steps[_currentStep]) <= 0.5f)
                {
                    _currentStep++;

                    if (_currentStep >= _steps.Length)
                    {
                        _direction = Vector3.zero;
                        return;
                    }
                }

                _direction = (_steps[_currentStep] - _stateMachine.transform.position).normalized;
            } else
            {
                _direction = Vector3.zero;
            }

            if (_direction != Vector3.zero)
            {
                if (Physics.BoxCast(_stateMachine.transform.position, Vector3.one * 0.5f, _direction, out RaycastHit hitInfo, _stateMachine.transform.rotation, 2f, Managers.GameManager.Instance.avoidanceLayerMask))
                {
                    _direction = (_direction * 2f - hitInfo.collider.transform.position).normalized * _data.AvoidanceForce;
                }
            }
        }

        public override void StateFixedUpdate()
        {
            if (_direction != Vector3.zero)
            {
                _movementController.Rotate(_direction);
                _movementController.Move(_stateMachine.transform.forward);
            }
        }

        public override void StateFinish()
        {
            if (_requestPathCoroutine != null)
            {
                _stateMachine.StopCoroutine(_requestPathCoroutine);
            }
        }

        private IEnumerator RequestPathCoroutine()
        {
            Pathfinding.Pathfinding.Instance.RequestPath(_stateMachine.transform.position, _target.position, OnPathProcessed);

            yield return new WaitForSeconds(_data.RequestPathTime);

            _requestPathCoroutine = _stateMachine.StartCoroutine(RequestPathCoroutine());
        }

        private void OnPathProcessed(Vector3 [] steps, bool success)
        {
            if (success)
            {
                _steps = steps;
                _currentStep = 0;
            } else
            {
                _steps = null;
                _currentStep = -1;
            }
        }

        private void DrawPath()
        {
            if (_steps != null && _steps.Length > 1)
            {
                Vector3 lastPosition = _steps[0];

                for (int i = 1; i < _steps.Length; i++)
                {
                    Debug.DrawLine(lastPosition, _steps[i], Color.green);
                    lastPosition = _steps[i];
                }
            }
        }
    }
}
