using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField]
        private Toggle _vsync;
        [SerializeField]
        private Toggle _headBob;

        private void Awake()
        {
            _vsync.isOn = Managers.GameSettings.Instance.VSyncCount > 0;
            _headBob.isOn = Managers.GameSettings.Instance.HeadBobActive;
        }

        public void ChangeVSync()
        {
            Managers.GameSettings.Instance.VSyncCount = _vsync.isOn ? 1 : 0;

            QualitySettings.vSyncCount = Managers.GameSettings.Instance.VSyncCount;
        }

        public void ChangeHeadBob()
        {
            Managers.GameSettings.Instance.HeadBobActive = _headBob.isOn;
        }
    } 
}
