using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ProceduralAnimation
{
    public class IKFeetSolver : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private LayerMask _raycastLayer;
        [SerializeField]
        private float _maxRayDistance;
        [SerializeField]
        private Vector2 _maxDistanceRange;
        [SerializeField]
        private IKTarget[] _targets;
        [SerializeField]
        private float _interpolationTime;
        [SerializeField]
        private float _curveMultiplier;
        private Queue<IKTarget> _movingQueue;
        private bool _isProcessingMovement = false;

        [SerializeField]
        private EntityEventController _eventController;

        private void Awake()
        {
            _movingQueue = new Queue<IKTarget>();

            foreach (IKTarget target in _targets)
            {
                if (GroundRaycast(target.RaycastOrigin.position, target.RaycastOrigin.transform.up * -1, out Vector3 hitPosition))
                {
                    target.IkTarget.position = hitPosition;
                }

                target.MaxDistance = Random.Range(_maxDistanceRange.x, _maxDistanceRange.y);
            }
        }

        private void Update()
        {
            foreach (IKTarget target in _targets)
            {
                if (GroundRaycast(target.RaycastOrigin.position, target.RaycastOrigin.transform.up * -1, out Vector3 hitPosition))
                {
                    target.PossiblePosition = hitPosition;

                    if (!target.IsMoving && Vector3.Distance(hitPosition, target.IkTarget.position) >= target.MaxDistance)
                    {
                        RequestMovement(target);
                    }
                }
            }
        }

        private void RequestMovement(IKTarget target)
        {
            target.IsMoving = true;
            _movingQueue.Enqueue(target);
            TryProcessNextMovement();
        }

        private void TryProcessNextMovement()
        {
            if (!_isProcessingMovement && _movingQueue.Count > 0)
            {
                _isProcessingMovement = true;
                StartCoroutine(MoveCoroutine(_movingQueue.Dequeue()));
            }
        }

        private IEnumerator MoveCoroutine(IKTarget target)
        {
            target.IsMoving = true;

            Vector3 currentPosition = target.IkTarget.position;
            Vector3 medianPosition = (target.IkTarget.position + target.PossiblePosition + Vector3.up * _curveMultiplier) / 2f;
            float timeElapsed = 0f;

            while (timeElapsed < _interpolationTime)
            {
                target.IkTarget.position = MathFunctions.QuadraticBezierCurve(currentPosition, medianPosition, target.PossiblePosition, timeElapsed / _interpolationTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            target.IkTarget.position = target.PossiblePosition;
            target.MaxDistance = Random.Range(_maxDistanceRange.x, _maxDistanceRange.y);
            
            target.IsMoving = false;

            _isProcessingMovement = false;
            TryProcessNextMovement();

            yield return null;
        }

        private bool GroundRaycast(Vector3 originPosition, Vector3 direction, out Vector3 hitPosition)
        {
            if (Physics.Raycast(originPosition, direction, out RaycastHit hit, _maxRayDistance, _raycastLayer))
            {
                hitPosition = hit.point;
                return true;
            } else
            {
                hitPosition = Vector3.zero;
                return false;
            }
        }       

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnDeath, OnDeath, EventExecutionOrder.Before);
            }
        }

        private void OnDeath(ref EntityEventContext ctx)
        {
            gameObject.SetActive(false);
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnDeath, OnDeath, EventExecutionOrder.Before);
            }
        }

        [System.Serializable]
        private class IKTarget
        {
            [SerializeField]
            private Transform _ikTarget;
            [SerializeField]
            private Transform _raycastOrigin;
            private float _maxDistance;
            private Vector3 _possiblePosition;
            private bool _isMoving = false;

            public Transform IkTarget => _ikTarget;
            public Transform RaycastOrigin => _raycastOrigin;
            public float MaxDistance { get => _maxDistance; set => _maxDistance = value; }
            public Vector3 PossiblePosition { get => _possiblePosition; set => _possiblePosition = value; }
            public bool IsMoving { get => _isMoving; set => _isMoving = value; }
        }
    } 
}
