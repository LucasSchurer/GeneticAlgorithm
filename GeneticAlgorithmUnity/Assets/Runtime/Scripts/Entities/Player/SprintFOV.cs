using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class SprintFOV : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _vCam;
        [SerializeField]
        private float _standardFOV;
        [SerializeField]
        private float _sprintingFOV;
        [SerializeField]
        private float _durationToReachStandardFOV;
        [SerializeField]
        private float _durationToReachSprintingFOV;

        private Coroutine _changeFOVCoroutine;

        public void StartSprinting()
        {
            if (_changeFOVCoroutine != null)
            {
                StopCoroutine(_changeFOVCoroutine);
            }

            _changeFOVCoroutine = StartCoroutine(ChangeFOVCoroutine(_sprintingFOV, _durationToReachSprintingFOV));
        }

        public void StopSprinting()
        {
            if (_changeFOVCoroutine != null)
            {
                StopCoroutine(_changeFOVCoroutine);
            }

            _changeFOVCoroutine = StartCoroutine(ChangeFOVCoroutine(_standardFOV, _durationToReachStandardFOV));
        }

        private IEnumerator ChangeFOVCoroutine(float desiredFOV, float duration)
        {
            float elapsedTime = 0f;

            float startFieldOfView = _vCam.m_Lens.FieldOfView;

            if (startFieldOfView == desiredFOV)
            {
                _changeFOVCoroutine = null;
                yield break;
            }

            while (elapsedTime < duration)
            {
                _vCam.m_Lens.FieldOfView = Mathf.Lerp(startFieldOfView, desiredFOV, elapsedTime / duration);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _vCam.m_Lens.FieldOfView = desiredFOV;

            _changeFOVCoroutine = null;
            yield return null;
        }

    } 
}
