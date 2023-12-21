using Game.Events;
using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class GameOver : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private Transform _gameOverScreen;

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
            if (GameManager.Instance)
            {
                GameManager.Instance.GetEventController()?.AddListener(GameEventType.OnGameOver, OnGameOver, EventExecutionOrder.After);
            }
        }

        private void OnGameOver(ref GameEventContext ctx)
        {
            _gameOverScreen.gameObject.SetActive(true);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void StopListening()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.GetEventController()?.RemoveListener(GameEventType.OnGameOver, OnGameOver, EventExecutionOrder.After);
            }
        }
    } 
}
