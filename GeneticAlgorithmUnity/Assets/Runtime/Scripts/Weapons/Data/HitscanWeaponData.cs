using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HitscanWeaponData")]
    public class HitscanWeaponData : WeaponData
    {
        [Header("Hitscan Settings")]
        [SerializeField]
        private float _hitRange;

        [Header("Graphical")]
        [SerializeField]
        private ParticleSystem _attackParticle;
        [SerializeField]
        private ParticleSystem _hitParticle;
        [SerializeField]
        private TrailRenderer _attackTrail;
        [SerializeField]
        private float _trailSpeed;

        public float HitRange => _hitRange;
        public ParticleSystem AttackParticle => _attackParticle;
        public ParticleSystem OnHitParticle => _hitParticle;
        public TrailRenderer AttackTrail => _attackTrail;
        public float TrailSpeed => _trailSpeed;
    }
}
