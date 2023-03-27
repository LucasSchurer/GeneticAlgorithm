using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class PathfindingTestAgent : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private Queue<Vector3> _steps;
        private Vector3[] _stepsTest;
        [SerializeField]
        private float _requestTime;
        [SerializeField]
        private bool _moveTowardsTarget = false;
        [SerializeField]
        private float _movementSpeed = 10f;
        [SerializeField]
        private float _minimumDistance = 1f;

        private Vector3 _currentStep;

        private Vector3 _direction;

        private void Start()
        {
            StartCoroutine(RequestPathCoroutine());
        }

        private void Update()
        {
            if (_moveTowardsTarget)
            {
                UpdateDirection();

                if (_direction != Vector3.zero)
                {
                    transform.Translate(_direction * _movementSpeed * Time.deltaTime);
                }
            }
        }

        private void UpdateDirection()
        {
            if (_currentStep != Vector3.zero)
            {
                if (Vector3.Distance(transform.position, _currentStep) <= _minimumDistance)
                {
                    if (_steps.Count > 0)
                    {
                        _currentStep = _steps.Dequeue();
                    } else
                    {
                        _currentStep = Vector3.zero;
                    }
                }

                _direction = (_currentStep - transform.position).normalized;
            } else
            {
                _direction = Vector3.zero;
            }
        }

        private IEnumerator RequestPathCoroutine()
        {
            Pathfinding.Instance.RequestPath(transform.position, _target.position, OnPathProcessed);

            if (_requestTime != 0)
            {
                yield return new WaitForSeconds(_requestTime);

                StartCoroutine(RequestPathCoroutine());
            }
        }

        private void OnPathProcessed(Vector3[] steps, bool success)
        {
            if (success)
            {
                _steps = new Queue<Vector3>(steps);
                _stepsTest = steps;

                if (_steps.Count > 0)
                {
                    _currentStep = _steps.Dequeue();
                }
            } else
            {
                _steps = null;
                _stepsTest = null;
                _currentStep = Vector3.zero;
            }
        }

        private void OnDrawGizmos()
        {
            if (_stepsTest != null && _stepsTest.Length > 1)
            {
                Vector3 lastPosition = _stepsTest[0];

                for (int i = 1; i < _stepsTest.Length; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(lastPosition, _stepsTest[i]);
                    Gizmos.DrawSphere(_stepsTest[i], 0.15f);

                    lastPosition = _stepsTest[i];
                }
            }
        }
    } 
}
