using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu]
    public class HitscanRangedWeaponData : WeaponData
    {
        [SerializeField]
        private float _hitRange;

        public float HitRange => _hitRange;
    }
}
