using System.Collections;
using UnityEngine;

namespace Game.Entities.AI
{
    public class ChaseState : State
    {
        private ChaseStateData _data;
        private AvoidanceData _avoidanceData;
        private MovementController _movementController;

        private Transform _target;
        private Vector3 _direction;

        private Coroutine _requestPathCoroutine;

        private Vector3[] _steps;
        private int _currentStep;

        public ChaseState(StateMachine stateMachine, ChaseStateData data) : base(stateMachine, data)
        {
            _data = data;
            _avoidanceData = _data.AvoidanceData;
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

                _direction = _avoidanceData.GetUpdatedDirection(_stateMachine.transform, _direction);
            } else
            {
                _direction = Vector3.zero;
            }
        }

        public override void StateFixedUpdate()
        {
            if (_direction != Vector3.zero)
            {
                _movementController.Rotate(_direction);
                _movementController.Move(_direction);
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

            Debug.DrawRay(_stateMachine.transform.position, _direction * 5f, Color.blue);
        }
    }
}
