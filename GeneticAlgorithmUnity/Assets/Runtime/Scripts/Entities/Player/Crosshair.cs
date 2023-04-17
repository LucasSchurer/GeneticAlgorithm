using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField]
        private float _hitIndicatorTime = 1f;
        private float _realHitIndicatorTime = 1f;
        private Coroutine _hitCoroutine;

        private Material _crosshairMaterial;
        private float _hitIndicatorTransparency;
        private const string HitTransparency = "_HitTransparency";        

        private void Awake()
        {
            _crosshairMaterial = GetComponent<MeshRenderer>().material;

            _hitIndicatorTransparency = _crosshairMaterial.GetFloat(HitTransparency);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Hit();
            }
        }

        private void Hit()
        {
            if (_hitCoroutine == null)
            {
                _realHitIndicatorTime = _hitIndicatorTime;
                _hitCoroutine = StartCoroutine(HitCoroutine());
            } else
            {
                _realHitIndicatorTime += _hitIndicatorTime;
            }
        }

        private IEnumerator HitCoroutine()
        {
            _hitIndicatorTransparency = 1f;

            _crosshairMaterial.SetFloat(HitTransparency, _hitIndicatorTransparency);

            float elapsedTime = 0f;

            while (elapsedTime <= _realHitIndicatorTime)
            {
                elapsedTime += Time.deltaTime;
                _hitIndicatorTransparency = Mathf.Lerp(1f, 0f, elapsedTime / _realHitIndicatorTime);
                _crosshairMaterial.SetFloat(HitTransparency, _hitIndicatorTransparency);
                yield return null;
            }

            _hitIndicatorTransparency = 0f;
            _crosshairMaterial.SetFloat(HitTransparency, _hitIndicatorTransparency);
            
            _hitCoroutine = null;
        }
    } 
}
