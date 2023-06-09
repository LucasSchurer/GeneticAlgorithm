using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class PhysicalProjectile : MonoBehaviour
    {
        protected GameObject _owner;        
        protected LayerMask _collisionLayer;
        protected LayerMask _damageLayer;
        protected Rigidbody _rigidbody;
        protected float _damage;

        public Rigidbody Rigidbody => _rigidbody;
    } 
}
