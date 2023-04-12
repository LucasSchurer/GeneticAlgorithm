using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ProceduralAnimation
{
    public class IKFootSolver : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _raycastLayer;
        [SerializeField]
        private float _maxRayDistance;
        [SerializeField]
        private Transform _raycastOrigin;
        [SerializeField]
        private Vector2 _maxDistanceRange;
        private float _maxDistance;
        [SerializeField]
        private float _moveSmoothness;

        private Vector3 _raycastOriginPosition;
        private Vector3 _possiblePosition;

        private bool _isMoving = false;

        private void Awake()
        {
            _raycastOriginPosition = _raycastOrigin.position;
            _raycastOriginPosition.y += 1f;

            if (GroundRaycast(_raycastOriginPosition, out Vector3 hitPosition))
            {
                transform.position = hitPosition;
            }

            _maxDistance = Random.Range(_maxDistanceRange.x, _maxDistanceRange.y);
        }

        private void Update()
        {
            if (!_isMoving)
            {
                if (GroundRaycast(_raycastOrigin.position, out Vector3 hitPosition))
                {
                    _possiblePosition = hitPosition;
                }

                if (Vector3.Distance(_possiblePosition, transform.position) >= _maxDistance)
                {
                    StartCoroutine(MoveCoroutine());
                }
            }
        }

        private IEnumerator MoveCoroutine()
        {
            _isMoving = true;

            Vector3 currentPosition = transform.position;
            float timeElapsed = 0f;

            while (timeElapsed <= _moveSmoothness)
            {
                transform.position = Vector3.Lerp(currentPosition, _possiblePosition, timeElapsed / _moveSmoothness);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = _possiblePosition;
            _maxDistance = Random.Range(_maxDistanceRange.x, _maxDistanceRange.y);
            _isMoving = false;

            yield return null;
        }

        private bool GroundRaycast(Vector3 originPosition, out Vector3 hitPosition)
        {
            if (Physics.Raycast(originPosition, Vector3.down, out RaycastHit hit, _maxRayDistance, _raycastLayer))
            {
                hitPosition = hit.point;
                return true;
            } else
            {
                hitPosition = Vector3.zero;
                return false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_possiblePosition, 0.01f);
        }
    } 
}
