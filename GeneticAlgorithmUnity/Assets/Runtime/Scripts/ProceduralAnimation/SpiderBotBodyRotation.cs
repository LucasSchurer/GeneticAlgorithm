using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ProceduralAnimation
{
    public class SpiderBotBodyRotation : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Pair of opposite legs targets that will be used to generate a normal through cross product")]
        private Transform[] _v1LegsTargets;
        private Vector3 _v1 = Vector3.zero;

        [SerializeField]
        [Tooltip("Pair of opposite legs targets that will be used to generate a normal through cross product")]
        private Transform[] _v2LegsTargets;
        private Vector3 _v2 = Vector3.zero;

        [SerializeField]
        private float _rotationSmoothness;

        private Vector3 _normal = Vector3.zero;
        private Vector3 _lastUp;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _lastUp = transform.up;
        }

        private void Update()
        {
            CalculateVectors();
            DrawDebugLines();
        }

        private void FixedUpdate()
        {
            transform.up = Vector3.Lerp(_lastUp, _normal, 1f / (_rotationSmoothness + 1f));
            _lastUp = transform.up;
        }

        private void CalculateVectors()
        {
            _v1 = (_v1LegsTargets[0].position - _v1LegsTargets[1].position);
            _v2 = (_v2LegsTargets[0].position - _v2LegsTargets[1].position);
            _normal = Vector3.Cross(_v1, _v2).normalized;
        }

        private void DrawDebugLines()
        {
            Debug.DrawLine(_v1LegsTargets[0].position, _v1LegsTargets[1].position, Color.blue);
            Debug.DrawLine(_v2LegsTargets[0].position, _v2LegsTargets[1].position, Color.red);
            Debug.DrawRay(transform.position, _normal);
        }
    } 
}
