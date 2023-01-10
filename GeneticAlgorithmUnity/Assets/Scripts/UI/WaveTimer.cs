using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI
{
    public class WaveTimer : MonoBehaviour
    {
        [Header("References")]
        private TextMeshProUGUI _timerLabel;
        private Managers.WaveManager _waveManager;

        private void Start()
        {
            _waveManager = Managers.WaveManager.Instance;

            _timerLabel = GetComponent<TextMeshProUGUI>();

            if (_waveManager == null || _timerLabel == null)
            {
                Destroy(gameObject);
            }

            StartCoroutine(UpdateTimerCoroutine());
        }

        private IEnumerator UpdateTimerCoroutine()
        {
            while (true)
            {
                _timerLabel.text = _waveManager.TimeRemaining.ToString("0");

                yield return new WaitForSeconds(0.5f);
            }
        }
    } 
}
