using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponManager : Singleton<WeaponManager>
    {
        [SerializeField]
        private WeaponTypeData[] _team1Weapons;
        private Dictionary<WeaponType, WeaponData> _team1WeaponsDict;
        [SerializeField]
        private WeaponTypeData[] _team2Weapons;
        private Dictionary<WeaponType, WeaponData> _team2WeaponsDict;

        public enum WeaponHolder
        {
            Team1,
            Team2
        }

        protected override void SingletonAwake()
        {
            _team1WeaponsDict = new Dictionary<WeaponType, WeaponData>();
            _team2WeaponsDict = new Dictionary<WeaponType, WeaponData>();

            foreach (WeaponTypeData weapon in _team1Weapons)
            {
                _team1WeaponsDict.TryAdd(weapon.Type, weapon.Data);
            }

            foreach (WeaponTypeData weapon in _team2Weapons)
            {
                _team2WeaponsDict.TryAdd(weapon.Type, weapon.Data);
            }
        }

        public WeaponData GetWeaponData(WeaponType type, WeaponHolder holder)
        {
            Dictionary<WeaponType, WeaponData> weaponsDict = holder == WeaponHolder.Team1 ? _team1WeaponsDict : _team2WeaponsDict;

            if (weaponsDict.TryGetValue(type, out WeaponData data))
            {
                return data;
            } else
            {
                return null;
            }
        }

        public IComponent AddWeaponComponent(GameObject go, WeaponType type, WeaponHolder holder)
        {
            Dictionary<WeaponType, WeaponData> weaponsDict = holder == WeaponHolder.Team1 ? _team1WeaponsDict : _team2WeaponsDict;

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
                    case WeaponType.OrbLauncher:
                        HomingOrbSpawner orbLauncher = go.AddComponent<HomingOrbSpawner>();
                        orbLauncher.SetData((HomingOrbSpawnerData)data);
                        return orbLauncher;
                }
            }

            return null;
        }

        public WeaponType GetRandomWeaponType(WeaponHolder holder)
        {
            if (holder == WeaponHolder.Team1)
            {
                return _team1Weapons[Random.Range(0, _team1Weapons.Length)].Type;
            } else
            {
                return _team2Weapons[Random.Range(0, _team2Weapons.Length)].Type;
            }
        }

        public WeaponType GetRandomWeaponType(WeaponHolder holder, System.Random rand)
        {
            if (holder == WeaponHolder.Team1)
            {
                return _team1Weapons[rand.Next(0, _team1Weapons.Length)].Type;
            }
            else
            {
                return _team1Weapons[rand.Next(0, _team2Weapons.Length)].Type;
            }
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