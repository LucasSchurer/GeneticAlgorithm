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

        private Transform _parentTransform;
        private Vector3 _adjustedPosition;

        private void Awake()
        {
            _parentTransform = transform.parent.transform;
        }

        private void Update()
        {
            _adjustedPosition = transform.position;
            _adjustedPosition.y = _parentTransform.position.y;

            if (Physics.Raycast(_adjustedPosition, Vector3.down, out RaycastHit hit, _maxRayDistance, _raycastLayer))
            {
                transform.position = hit.point;
            }
        }
    } 
}
