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

        public IComponent AddWeaponComponent(GameObject go, WeaponType type, WeaponHolder holder)
        {
            Dictionary<WeaponType, WeaponData> weaponsDict = holder == WeaponHolder.Player ? _playerWeaponsDict : _enemyWeaponsDict;

            if (weaponsDict.TryGetValue(type, out WeaponData data))
            {
                switch (type)
                {
                    case WeaponType.None:
                        break;
                    case WeaponType.Pistol:
                        Pistol pistol = go.AddComponent<Pistol>();
                        pistol.SetData((HitscanWeaponData)data);
                        return pistol;
                    case WeaponType.GrenadeLauncher:
                        GrenadeLauncher grenadeLauncher = go.AddComponent<GrenadeLauncher>();
                        grenadeLauncher.SetData((GrenadeLauncherData)data);
                        return grenadeLauncher;
                    case WeaponType.DamageOrbs:
                        HomingOrbSpawner damageOrbs = go.AddComponent<HomingOrbSpawner>();
                        damageOrbs.SetData((HomingOrbSpawnerData)data);
                        return damageOrbs;
                    case WeaponType.HealingOrbs:
                        HomingOrbSpawner healingOrbs = go.AddComponent<HomingOrbSpawner>();
                        healingOrbs.SetData((HomingOrbSpawnerData)data);
                        return healingOrbs;
                    case WeaponType.ShieldDeployer:
                        ShieldDeployer shieldDeployer = go.AddComponent<ShieldDeployer>();
                        shieldDeployer.SetData((ShieldDeployerData)data);
                        return shieldDeployer;
                    case WeaponType.Nuke:
                        Nuke nuke = go.AddComponent<Nuke>();
                        nuke.SetData((NukeData)data);
                        return nuke;
                }
            }

            return null;
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