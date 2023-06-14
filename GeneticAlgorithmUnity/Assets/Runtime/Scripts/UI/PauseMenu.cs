using Game.Events;
using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class PauseMenu : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private Transform _settings;        
        [SerializeField]
        private Transform _pause;

        public enum Menu
        {
            Settings,
            Pause
        }

        public void ChangeMenu(Menu menu)
        {
            _settings.gameObject.SetActive(menu == Menu.Settings);
            _pause.gameObject.SetActive(menu == Menu.Pause);
        }

        public void Settings()
        {
            ChangeMenu(Menu.Settings);
        }

        public void Pause()
        {
            ChangeMenu(Menu.Pause);
        }

        public void Resume()
        {
            _settings.gameObject.SetActive(false);
            _pause.gameObject.SetActive(false);

            GameManager.Instance.ResumeGame(false);
        }

        private void OnPause(ref GameEventContext ctx)
        {
            ChangeMenu(Menu.Pause);
        }

        private void OnResume(ref GameEventContext ctx)
        {
            Resume();
        }

        public void StartListening()
        {
            GameManager.Instance.GetEventController()?.AddListener(GameEventType.OnPause, OnPause);
            GameManager.Instance.GetEventController()?.AddListener(GameEventType.OnResume, OnResume);
        }

        public void StopListening()
        {
            GameManager.Instance.GetEventController()?.RemoveListener(GameEventType.OnPause, OnPause);
            GameManager.Instance.GetEventController()?.RemoveListener(GameEventType.OnResume, OnResume);
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