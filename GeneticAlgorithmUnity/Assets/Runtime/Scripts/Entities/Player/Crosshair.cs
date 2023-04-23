using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public class Crosshair : MonoBehaviour, IEventListener
    {
        public enum CrosshairState
        {
            Default,
            Standing,
            Running
        }

        [Header("General Settings")]
        [SerializeField]
        private float _hitIndicatorTime = 1f;

        [Header("Scale Settings")]
        [SerializeField]
        private Vector3 _defaultScale = new Vector3(0.01f, 0.01f, 0.01f);
        [SerializeField]
        private Vector3 _runningScale = new Vector3(0.03f, 0.03f, 0.03f);
        [SerializeField]
        private Vector3 _standingScale = new Vector3(0.01f, 0.01f, 0.01f);
        [SerializeField]
        private float _scaleSpeed = 2f;

        [Header("References")]
        [SerializeField]
        private EntityEventController _playerEventController;
        [SerializeField]
        private Rigidbody _playerRb;

        private Material _crosshairMaterial;
        
        private const string HitTransparency = "_HitTransparency";
        private float _hitIndicatorTransparency;
        
        private Coroutine _hitCoroutine;
        private float _realHitIndicatorTime = 1f;

        private Vector3 _targetScale = Vector3.zero;
        private CrosshairState _currentState = CrosshairState.Default;
        private CrosshairState _oldState = CrosshairState.Default;

        private void Awake()
        {
            _crosshairMaterial = GetComponent<MeshRenderer>().material;

            _hitIndicatorTransparency = _crosshairMaterial.GetFloat(HitTransparency);

            ChangeState(CrosshairState.Default);
        }

        public void SetState(CrosshairState state)
        {
            _oldState = _currentState;
            _currentState = state;

            if (_currentState != _oldState)
            {
                ChangeState(_currentState);
            }
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _scaleSpeed * Time.deltaTime);
        }

        private void ChangeState(CrosshairState state)
        {
            switch (state)
            {
                case CrosshairState.Default:
                    _targetScale = _defaultScale;
                    break;
                case CrosshairState.Standing:
                    _targetScale = _standingScale;
                    break;
                case CrosshairState.Running:
                    _targetScale = _runningScale;
                    break;
            }
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
