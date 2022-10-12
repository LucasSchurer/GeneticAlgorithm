using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu]
    public class Weapon : ScriptableObject
    {
        [Header("Weapon")]
        [Tooltip("Time between weapon uses")]
        public float cooldown;
        public float damage;
    }
}

