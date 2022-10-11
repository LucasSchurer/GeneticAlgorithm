using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu]
    public class RangedWeapon : Weapon
    {
        public Projectiles.Projectile baseProjectile;
    } 
}
