using Game.Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private BehaviourType _behaviourType;

        private Vector3 _direction;
        private Transform _playerPosition;

        private Transform _target;
        private Vertex _lastStep;
        private Queue<Vertex> _steps;
        private Vertex _currentStep;
        [SerializeField]
        private float _maxTargetDistanceFromLastVertex = 1f;

        private MovementController _movementController;
        [SerializeField]
        private float _acceptableMinimumStepDistance = 0.4f;

        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            _target = _playerPosition;
            Pathfinding.Pathfinding.Instance.RequestPath(transform.position, _target.position, OnPathProcessed);
        }

        private void OnPathProcessed(Vertex[] steps, bool sucess)
        {
            if (sucess)
            {
                _lastStep = steps[steps.Length - 1];
                _steps = new Queue<Vertex>(steps);
                _currentStep = _steps.Dequeue();
            }
        }

        private void Update()
        {
            UpdateDirection();

            if (_lastStep != null)
            {
                Vector3 lastStepPosition = _lastStep.Position;
                lastStepPosition.y = _target.position.y;

                if (Vector3.Distance(_target.position, lastStepPosition) > _maxTargetDistanceFromLastVertex)
                {
                    _steps.Clear();
                    _currentStep = null;
                    Pathfinding.Pathfinding.Instance.RequestPath(transform.position, _target.position, OnPathProcessed);
                }
            }
        }

        private void FixedUpdate()
        {
            _movementController?.Move(_direction);
            _movementController?.Rotate(_direction);
        }

        private void UpdateDirection()
        {
            if (_steps != null && _currentStep != null && _steps.Count > 0)
            {
                _direction = (_currentStep.Position - transform.position).normalized;

                Vector3 currentStepPosition = _currentStep.Position;
                currentStepPosition.y = transform.position.y;

                float distance = Vector3.Distance(transform.position, currentStepPosition);

                if (distance <= _acceptableMinimumStepDistance)
                {
                    if (_steps.Count > 0)
                    {
                        _currentStep = _steps.Dequeue();
                    }
                }
            } else
            {
                _direction = Vector3.zero;
            }
        }
    } 
}
