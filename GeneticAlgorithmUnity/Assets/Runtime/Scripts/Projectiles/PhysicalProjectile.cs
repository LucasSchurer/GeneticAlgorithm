using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class PhysicalProjectile : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _owner;
        [SerializeField]
        protected LayerMask _collisionLayer;
        [SerializeField]
        protected LayerMask _damageLayer;
        [SerializeField]
        protected Rigidbody _rigidbody;
        [SerializeField]
        protected float _damage;

        public Rigidbody Rigidbody => _rigidbody;

        public virtual void Initialize(float damage, LayerMask collisionLayer, LayerMask damageLayer, GameObject owner)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _damage = damage;
            _collisionLayer = collisionLayer;
            _damageLayer = damageLayer;
            _owner = owner;
        }
    } 
}
