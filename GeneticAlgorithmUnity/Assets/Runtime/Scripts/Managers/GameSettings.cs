using Game.Weapons;
using UnityEngine;

namespace Game.Managers
{
    public class GameSettings : Singleton<GameSettings>
    {
        private bool _headBobActive = true;
        [SerializeField]
        private int _vSyncCount = 1;
        [SerializeField]
        private WeaponType[] _selectedWeapons = new WeaponType[3] { WeaponType.Pistol, WeaponType.GrenadeLauncher, WeaponType.DamageOrbs };
        [SerializeField]
        private int _traitSelectionAmount = 3;

        public int VSyncCount { get => _vSyncCount; set => _vSyncCount = Mathf.Clamp(value, 0, 4); }
        public bool HeadBobActive { get => _headBobActive; set => _headBobActive = value; }
        public WeaponType[] SelectedWeapons => _selectedWeapons;
        public int TraitSelectionAmount => _traitSelectionAmount;

        protected override void SingletonAwake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void SetSelectedWeapons(WeaponType[] weapons)
        {
            _selectedWeapons = weapons;
        }
    } 
}