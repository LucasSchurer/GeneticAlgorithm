using Game.Weapons;
using UnityEngine;

namespace Game.Managers
{
    public class GameSettings : Singleton<GameSettings>
    {
        public bool _headBobActive = true;
        public bool _cameraShake = true;
        [SerializeField]
        private int _vSyncCount = 1;
        [SerializeField]
        private WeaponType[] _selectedWeapons = new WeaponType[3] { WeaponType.Pistol, WeaponType.GrenadeLauncher, WeaponType.DamageOrbs };

        public int VSyncCount => Mathf.Clamp(_vSyncCount, 0, 4);
        public WeaponType[] SelectedWeapons => _selectedWeapons;

        protected override void SingletonAwake()
        {
            
        }
    } 
}