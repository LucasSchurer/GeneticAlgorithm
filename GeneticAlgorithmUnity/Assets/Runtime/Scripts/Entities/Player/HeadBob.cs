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
        private float _standingAmplitude = 0f;
        [SerializeField]
        private float _standingFrequency = 0f;
        [SerializeField]
        private float _walkingAmplitude = 0f;
        [SerializeField]
        private float _walkingFrequency = 0f;
        [SerializeField]
        private float _runningFrequency = 0f;
        [SerializeField]
        private float _runningAmplitude = 0f;
        [SerializeField]
        private Rigidbody _player;
        [SerializeField]
        private AnimationCurve _amplitudeCurve;
        [SerializeField]
        private AnimationCurve _frequencyCurve;
        [SerializeField]
        private float _maxSpeed;
        [SerializeField]
        private float _amplitudeMultiplier;
        [SerializeField]
        private float _frequencyMultiplier;
        [SerializeField]
        private Vector3 _playerVelocity;

        private void Awake()
        {
            _noise = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            _playerVelocity = _player.velocity;
            Test();
        }

        private void UpdateFrequencyAmplitude()
        {
            switch (_playerMovementController.CurrentStatus)
            {
                case MovementController.Status.Standing:
                    ChangeFrequencyAmplitude(_standingFrequency, _standingAmplitude);
                    break;
                case MovementController.Status.Walking:
                    ChangeFrequencyAmplitude(_walkingFrequency, _walkingAmplitude);
                    break;
                case MovementController.Status.Running:
                    ChangeFrequencyAmplitude(_runningFrequency, _runningAmplitude);
                    break;
                case MovementController.Status.Air:
                    ChangeFrequencyAmplitude(0f, 0f);
                    break;
            }
        }

        private void ChangeFrequencyAmplitude(float frequency, float amplitude)
        {
            _noise.m_FrequencyGain = frequency;
            _noise.m_AmplitudeGain = amplitude;
        }

        private void Test()
        {
            float frequency = 0f;
            float amplitude = 0f;

            float velocityZX = Mathf.Abs(_player.velocity.z) + Mathf.Abs(_playerVelocity.x);

            if (true)
            {
                float clampedZX = Mathf.Clamp(velocityZX, 0f, _maxSpeed);

                frequency = _frequencyCurve.Evaluate(clampedZX / _maxSpeed * _frequencyMultiplier);
                amplitude = _amplitudeCurve.Evaluate(clampedZX / _maxSpeed * _amplitudeMultiplier);
            }

            ChangeFrequencyAmplitude(frequency, amplitude);
        }
    }
}
