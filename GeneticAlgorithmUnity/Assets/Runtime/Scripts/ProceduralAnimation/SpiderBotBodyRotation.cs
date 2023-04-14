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
        [SerializeField]
        private float _heightAdjustmentTime;
        [SerializeField]
        private float _heightAdjusmentThreshold;
        [SerializeField]
        private float _rotationThreshold;

        [SerializeField]
        private float _heightOffset;

        private Vector3 _normal = Vector3.zero;

        [SerializeField]
        private bool _rotate;
        [SerializeField]
        private bool _adjustHeight;

        private Coroutine _ajdustHeightCoroutine;

        private void Update()
        {
            CalculateVectors();
            DrawDebugLines();
        }

        private void FixedUpdate()
        {
            if (_rotate)
            {
                Rotate();
            }
            
            if (_adjustHeight)
            {
                AdjustHeight();
            }
        }

        private void AdjustHeight()
        {
            Vector3 v1Median = (_v1LegsTargets[0].position + _v1LegsTargets[1].position) / 2f;
            Vector3 v2Median = (_v2LegsTargets[0].position + _v2LegsTargets[1].position) / 2f;

            Vector3 averagePosition = new Vector3(transform.position.x, ((v1Median.y + v2Median.y) / 2f) + _heightOffset, transform.position.z);

            if (Vector3.Distance(averagePosition, transform.position) >= _heightAdjusmentThreshold)
            {
                if (_ajdustHeightCoroutine != null)
                {
                    StopCoroutine(_ajdustHeightCoroutine);
                }

                _ajdustHeightCoroutine = StartCoroutine(AdjustHeightCoroutine(averagePosition));
            }
        }

        private IEnumerator AdjustHeightCoroutine(Vector3 desiredPosition)
        {
            Vector3 currentPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < _heightAdjustmentTime)
            {
                Vector3 newPosition = Vector3.Lerp(currentPosition, desiredPosition, elapsedTime / _heightAdjustmentTime);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = desiredPosition;
        }

        private void Rotate()
        {
            if (Vector3.Distance(transform.position, _normal) > _rotationThreshold)
            {
                transform.up = Vector3.Lerp(transform.up, _normal, 1f / (_rotationSmoothness + 1f));
            }
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
