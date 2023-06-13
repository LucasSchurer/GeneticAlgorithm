using UnityEngine;

namespace Game.Managers
{
    public class GameSettings : Singleton<GameSettings>
    {
        public bool _headBobActive = true;
        public bool _cameraShake = true;
        [SerializeField]
        private int _vSyncCount = 1;

        public int VSyncCount => Mathf.Clamp(_vSyncCount, 0, 4);

        protected override void SingletonAwake()
        {
            
        }
    } 
}