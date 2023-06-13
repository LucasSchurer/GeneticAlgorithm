using Cinemachine;
using Game.Entities.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class HeadBob : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _vCam;
        private CinemachineBasicMultiChannelPerlin _noise;
        [SerializeField]
        private MovementController _playerMovementController;
        [SerializeField]
        private Rigidbody _player;
        [SerializeField]
        private AnimationCurve _amplitudeCurve;
        [SerializeField]
        private AnimationCurve _frequencyCurve;
        [SerializeField]
        private float _maxSpeed;

        private Managers.GameSettings _gameSettings;

        private void Awake()
        {
            _noise = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _gameSettings = Managers.GameSettings.Instance;
        }

        private void Update()
        {
            if (_gameSettings._headBobActive)
            {
                UpdateFrequencyAmplitude();
            }
            else if (_noise.m_AmplitudeGain != 0 || _noise.m_FrequencyGain != 0)
            {
                _noise.m_FrequencyGain = 0f;
                _noise.m_AmplitudeGain = 0f;
            }
        }

        private void UpdateFrequencyAmplitude()
        {
            float frequency = 0f;
            float amplitude = 0f;

            float velocityZX = Mathf.Abs(_player.velocity.z) + Mathf.Abs(_player.velocity.x);

            float clampedZX = Mathf.Clamp(velocityZX, 0f, _maxSpeed);

            frequency = _frequencyCurve.Evaluate(clampedZX / _maxSpeed);
            amplitude = _amplitudeCurve.Evaluate(clampedZX / _maxSpeed);

            _noise.m_FrequencyGain = frequency;
            _noise.m_AmplitudeGain = amplitude;
        }
    }
}
