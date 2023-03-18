using Game.Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        /*private Vector3 _lastStep;
        private Queue<Vector3> _steps;
        private Vector3 _currentStep;
        [SerializeField]
        private float _maxTargetDistanceFromLastVertex = 1f;*/

        private MovementController _movementController;
        /*[SerializeField]
        private float _acceptableMinimumStepDistance = 0.4f;*/

        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            _target = _playerPosition;
            /*Pathfinding.Pathfinding.Instance.RequestPath(transform.position, _target.position, OnPathProcessed);*/
        }

        /*private void OnPathProcessed(Vector3[] steps, bool sucess)
        {
            if (sucess)
            {
                _lastStep = steps[steps.Length - 1];
                _steps = new Queue<Vector3>(steps);
                _currentStep = _steps.Dequeue();
            }
        }*/

/*        private void OnDrawGizmos()
        {
            if (_steps != null)
            {
                foreach (Vector3 step in _steps.ToList())
                {
                    Gizmos.DrawWireSphere(step, 0.5f);
                }
            }
        }*/

        private void Update()
        {
            UpdateDirection();

/*            if (_lastStep != null)
            {
                Vector3 lastStepPosition = _lastStep;
                lastStepPosition.y = _target.position.y;

                if (Vector3.Distance(_target.position, lastStepPosition) > _maxTargetDistanceFromLastVertex)
                {
                    _steps.Clear();
                    _currentStep = Vector3.zero;
                    Pathfinding.Pathfinding.Instance.RequestPath(transform.position, _target.position, OnPathProcessed);
                }
            }*/
        }

        private void FixedUpdate()
        {
            if (_movementController)
            {
                _movementController.Rotate(_direction);
                _movementController.Move(_direction);
            }
        }

        private void UpdateDirection()
        {
            // TODO
            // Fix A* paths to remove sharp turns
            // or implement a flow field pathfinding 
/*            if (_steps != null && _currentStep != null)
            {
                _direction = (_currentStep - transform.position).normalized;

                Vector3 currentStepPosition = _currentStep;
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
            }*/
        }
    } 
}
