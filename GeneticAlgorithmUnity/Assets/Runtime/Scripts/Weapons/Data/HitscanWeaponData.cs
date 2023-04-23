using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HitscanWeaponData")]
    public class HitscanWeaponData : WeaponData
    {
        [Header("General Settings")]
        [SerializeField]
        private float _hitRange;

        [Header("Graphical")]
        [SerializeField]
        private ParticleSystem _attackParticle;
        [SerializeField]
        private ParticleSystem _hitParticle;

        public float HitRange => _hitRange;
        public ParticleSystem AttackParticle => _attackParticle;
        public ParticleSystem OnHitParticle => _hitParticle;
    }
}
