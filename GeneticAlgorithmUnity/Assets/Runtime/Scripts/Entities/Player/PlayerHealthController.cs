using Cinemachine;
using Game.Entities.Shared;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Entities.Player
{
    public class PlayerHealthController : HealthController
    {
        [Header("Damage and Healing Overlay Settings")]
        [SerializeField]
        private Volume _volume;
        [SerializeField]
        private Color _damageColor;
        [SerializeField]
        private Color _healingColor;
        [SerializeField]
        [Range(0f, 1f)]
        private float _intensity;
        [SerializeField]
        private float _durationToReachIntensity;
        [SerializeField]
        private float _durationToReach0Intensity;
        
        [Header("Damage Camera Shake")]
        [SerializeField]
        private CinemachineImpulseSource _impulseSource;

        private Vignette _vignetteEffect;

        private Coroutine _showingVignetteEffect;

        protected override void Start()
        {
            base.Start();

            VolumeProfile profile = _volume.profile;

            profile.TryGet(out _vignetteEffect);
        }

        protected override void Healed()
        {
            base.Healed();

            if (_showingVignetteEffect == null)
            {
                _showingVignetteEffect = StartCoroutine(ShowVignetteEffect(_healingColor));
            }
        }

        protected override void Damaged()
        {
            base.Damaged();

            if (_showingVignetteEffect == null)
            {
                _showingVignetteEffect = StartCoroutine(ShowVignetteEffect(_damageColor));
            }

            _impulseSource.GenerateImpulse();
        }

        private IEnumerator ShowVignetteEffect(Color color)
        {
            _vignetteEffect.color.Override(color);

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

            _showingVignetteEffect = null;
        }
    } 
}
