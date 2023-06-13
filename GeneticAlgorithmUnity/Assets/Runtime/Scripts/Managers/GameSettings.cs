using UnityEngine;

namespace Game.Managers
{
    public class GameSettings : Singleton<GameSettings>
    {
        public bool _headBobActive = true;
        public bool _cameraShake = true;

        protected override void SingletonAwake()
        {
            
        }
    } 
}