using UnityEngine;
using Game.Projectiles;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HomingOrbSpawnerData")]
    public class HomingOrbSpawnerData : WeaponData
    {
        [Header("Orb Settings")]
        [SerializeField]
        private HomingOrb _orb;
        [SerializeField]
        private float _orbSpeed;
        [SerializeField]
        [Tooltip("If checked, bots won't lock a position and will follow the target")]
        private bool _hardLock;
        [SerializeField]
        private float _findTargetRadius;
        [SerializeField]
        private float _startLockingTime;
        [SerializeField]
        private float _startSeekingTime;

        [Header("Orb Material References")]
        [SerializeField]
        private Material _orbMaterial;
        [SerializeField]
        private Material _trailMaterial;
        [SerializeField]
        private Material _hitMaterial;

        public HomingOrb Orb => _orb;
        public float OrbSpeed => _orbSpeed;
        public bool HardLock => _hardLock;
        public float FindTargetRadius => _findTargetRadius;
        public float StartLockingTime => _startLockingTime;
        public float StartSeekingTime => _startSeekingTime;
        public Material OrbMaterial => _orbMaterial;
        public Material TrailMaterial => _trailMaterial;
        public Material HitMaterial => _hitMaterial;
    }
}

