using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu]
    public class RangedWeaponData : WeaponData
    {
        [Header("Ranged Weapon")]
        public Projectiles.Projectile baseProjectile;
        [Tooltip("The random horizontal value applied to the projectile when its instantiated")]
        public float projectileVariation;
    } 
}
