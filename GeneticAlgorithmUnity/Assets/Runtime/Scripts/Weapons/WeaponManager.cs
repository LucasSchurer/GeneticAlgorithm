using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponManager : Singleton<WeaponManager>
    {
        [SerializeField]
        private WeaponTypeData[] _playerWeapons;
        private Dictionary<WeaponType, WeaponData> _playerWeaponsDict;
        [SerializeField]
        private WeaponTypeData[] _enemyWeapons;
        private Dictionary<WeaponType, WeaponData> _enemyWeaponsDict;

        public enum WeaponHolder
        {
            Player,
            Enemy
        }

        protected override void SingletonAwake()
        {
            _playerWeaponsDict = new Dictionary<WeaponType, WeaponData>();
            _enemyWeaponsDict = new Dictionary<WeaponType, WeaponData>();

            foreach (WeaponTypeData weapon in _playerWeapons)
            {
                _playerWeaponsDict.TryAdd(weapon.Type, weapon.Data);
            }

            foreach (WeaponTypeData weapon in _enemyWeapons)
            {
                _enemyWeaponsDict.TryAdd(weapon.Type, weapon.Data);
            }
        }

        public void AddWeaponComponent(GameObject go, WeaponType type, WeaponHolder holder)
        {
            Dictionary<WeaponType, WeaponData> weaponsDict = holder == WeaponHolder.Player ? _playerWeaponsDict : _enemyWeaponsDict;

            if (weaponsDict.TryGetValue(type, out WeaponData data))
            {
                switch (type)
                {
                    case WeaponType.None:
                        break;
                    case WeaponType.Pistol:
                        go.AddComponent<Pistol>().SetData((HitscanWeaponData)data);
                        break;
                    case WeaponType.GrenadeLauncher:
                        go.AddComponent<GrenadeLauncher>().SetData((GrenadeLauncherData)data);
                        break;
                    case WeaponType.DamageOrbs:
                        go.AddComponent<HomingOrbSpawner>().SetData((HomingOrbSpawnerData)data);
                        break;
                    case WeaponType.HealingOrbs:
                        go.AddComponent<HomingOrbSpawner>().SetData((HomingOrbSpawnerData)data);
                        break;
                    case WeaponType.ShieldDeployer:
                        go.AddComponent<ShieldDeployer>().SetData((ShieldDeployerData)data);
                        break;
                    case WeaponType.Nuke:
                        go.AddComponent<Nuke>().SetData((NukeData)data);
                        break;
                }
            }
        }

        public WeaponType GetRandomWeaponType(WeaponHolder holder)
        {
            return _enemyWeapons[Random.Range(0, _enemyWeapons.Length - 1)].Type;
        }

        [System.Serializable]
        private struct WeaponTypeData
        {
            [SerializeField]
            private WeaponType _type;
            [SerializeField]
            private WeaponData _data;

            public WeaponType Type => _type;
            public WeaponData Data => _data;
        }
    }
}