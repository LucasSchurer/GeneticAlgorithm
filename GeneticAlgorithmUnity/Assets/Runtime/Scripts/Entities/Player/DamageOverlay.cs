using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Entities.Player
{
    public class DamageOverlay : MonoBehaviour
    {
        [SerializeField]
        private EntityEventController _playerEventController;
        [SerializeField]
        private Volume _damageVolume;
        [SerializeField]
        [Range(0f, 1f)]
        private float _intensity;
        [SerializeField]
        private float _durationToReachIntensity;
        [SerializeField]
        private float _durationToReach0Intensity;

        private Vignette _vignetteEffect;

        private Coroutine _showDamageCoroutine;

        private void Start()
        {
            VolumeProfile profile = _damageVolume.profile;

            profile.TryGet(out _vignetteEffect);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (_showDamageCoroutine == null)
                {
                    _showDamageCoroutine = StartCoroutine(ShowDamageCoroutine());
                }   
            }
        }

        private void OnPlayerHealthChange(ref EntityEventContext ctx)
        {
            
        }

        private IEnumerator ShowDamageCoroutine()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _durationToReachIntensity)
            {
                float intensity = Mathf.Lerp(0f, _intensity, elapsedTime / _durationToReachIntensity);

                _vignetteEffect.intensity.Override(intensity);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            elapsedTime = 0f;

            while (elapsedTime < _durationToReach0Intensity)
            {
                float intensity = Mathf.Lerp(_intensity, 0f, elapsedTime / _durationToReach0Intensity);

                _vignetteEffect.intensity.Override(intensity);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _vignetteEffect.intensity.Override(0f);

            _showDamageCoroutine = null;
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
                _playerEventController.AddListener(EntityEventType.OnHealthChange, OnPlayerHealthChange, EventExecutionOrder.After);
            }
        }

        public void StopListening()
        {
            if (_playerEventController)
            {
                _playerEventController.RemoveListener(EntityEventType.OnHealthChange, OnPlayerHealthChange, EventExecutionOrder.After);
            }
        }
    } 
}
