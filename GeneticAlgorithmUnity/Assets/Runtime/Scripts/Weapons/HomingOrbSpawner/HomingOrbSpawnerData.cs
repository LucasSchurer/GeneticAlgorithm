using UnityEngine;
using Game.Projectiles;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HomingOrbSpawnerData")]
    public class HomingOrbSpawnerData : WeaponData
    {
        [Header("Layers")]
        [SerializeField]
        private LayerMask _hitLayer;
        [SerializeField]
        private LayerMask _targetLayer;

        [Header("Orb Settings")]
        [SerializeField]
        private HomingOrb _orb;
        [SerializeField]
        private int _orbAmount;
        [SerializeField]
        private float _spawnInterval;
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
        [SerializeField]
        private bool _canHitSelf = false;
        [SerializeField]
        private float _maxDistance = 0f;

        [Header("Orb Material References")]
        [SerializeField]
        private Material _orbMaterial;
        [SerializeField]
        private Material _trailMaterial;
        [SerializeField]
        private Material _hitMaterial;

        [Header("Orb Custom Actions")]
        [SerializeField]
        private HomingOrbOnHit _orbOnHit;
        [SerializeField]
        private HomingOrbSpawn _orbSpawn;

        public LayerMask HitLayer => _hitLayer;
        public LayerMask TargetLayer => _targetLayer;
        public HomingOrb Orb => _orb;
        public int OrbAmount => _orbAmount;
        public float SpawnInterval => _spawnInterval;
        public float OrbSpeed => _orbSpeed;
        public bool HardLock => _hardLock;
        public float FindTargetRadius => _findTargetRadius;
        public float StartLockingTime => _startLockingTime;
        public float StartSeekingTime => _startSeekingTime;
        public Material OrbMaterial => _orbMaterial;
        public Material TrailMaterial => _trailMaterial;
        public Material HitMaterial => _hitMaterial;
        public HomingOrbOnHit OrbOnHit => _orbOnHit;
        public HomingOrbSpawn OrbSpawn => _orbSpawn;
        public bool CanHitSelf => _canHitSelf;
        public float MaxDistance => _maxDistance;
    }
}

