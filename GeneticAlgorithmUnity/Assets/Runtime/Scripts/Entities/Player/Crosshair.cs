using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public class Crosshair : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private float _hitIndicatorTime = 1f;
        [SerializeField]
        private EntityEventController _playerEventController;

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

        private void OnHitDealt(ref EntityEventContext ctx)
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
            if (_playerEventController)
            {
                _playerEventController.AddListener(EntityEventType.OnHitDealt, OnHitDealt, EventExecutionOrder.After);
            }
        }

        public void StopListening()
        {
            if (_playerEventController)
            {
                _playerEventController.RemoveListener(EntityEventType.OnHitDealt, OnHitDealt, EventExecutionOrder.After);
            }
        }
    } 
}
