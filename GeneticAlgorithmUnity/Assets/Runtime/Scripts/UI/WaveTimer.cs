using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Events;
using Game.Managers;
using System;

namespace Game.UI
{
    public class WaveTimer : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private TextMeshProUGUI _timerLabel;
        [SerializeField]
        private TextMeshProUGUI _currentWaveLabel;

        private WaveManager _waveManager;
        private bool hasStarted = false;

        private int _currentWave = 0;

        private void Start()
        {
            _waveManager = WaveManager.Instance;

            if (_waveManager == null || _timerLabel == null)
            {
                Destroy(gameObject);
            }

            hasStarted = true;
        }

        private IEnumerator UpdateTimerCoroutine()
        {
            while (true)
            {
                if (!hasStarted)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                float timeRemaining = _waveManager.TimeRemaining;

                if (timeRemaining < 6.5f)
                {
                    _timerLabel.color = Color.red;
                }

                _timerLabel.text = "Tempo restante: " + timeRemaining.ToString("0");

                if (!_waveManager.IsWaveActive)
                {
                    break;
                }

                yield return new WaitForSeconds(0.5f);
            }

            _timerLabel.color = Color.white;

            yield return null;
        }

        public void StartListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.GetEventController().AddListener(GameEventType.OnWaveStart, StartTimer, EventExecutionOrder.After);
            }
        }

        private void StartTimer(ref GameEventContext ctx)
        {
            _currentWave++;

            _currentWaveLabel.text = "Wave Atual: " + _currentWave.ToString();

            StopAllCoroutines();
            StartCoroutine(UpdateTimerCoroutine());
        }

        public void StopListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveStart, StartTimer, EventExecutionOrder.After);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }
    } 
}
