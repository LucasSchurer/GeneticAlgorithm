using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu]
    public class WeaponData : ScriptableObject
    {
        [Header("Weapon")]
        [Tooltip("Time between weapon uses")]
        public float cooldown;
        public float damage;
    }
}

