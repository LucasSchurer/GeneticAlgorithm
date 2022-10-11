using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Projectile : ScriptableObject
    {
        public float speed;
        public float damage;
        public float timeToDespawn;
    } 
}
