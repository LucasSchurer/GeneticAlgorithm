using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HitscanWeaponData")]
    public class HitscanWeaponData : WeaponData
    {
        [SerializeField]
        private float _hitRange;

        public float HitRange => _hitRange;
    }
}
