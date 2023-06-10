using UnityEngine;
using Game.Projectiles;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/DamageHomingOrbSpawnerData")]
    public class DamageHomingOrbSpawnerData : HomingOrbSpawnerData
    {
        [Header("Orb Spawn Settings")]
        [SerializeField]
        private Vector2 _ySpawnRange;
        [SerializeField]
        private Vector2 _zSpawnRange;
        [SerializeField]
        private Vector2 _xSpawnRange;

        public Vector2 YSpawnRange => _ySpawnRange;
        public Vector2 ZSpawnRange => _zSpawnRange;
        public Vector2 XSpawnRange => _xSpawnRange;
    }
}

