using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public abstract class SpawnProjectileEffect<Context> : Effect<Context>
        where Context: EventContext
    {
        [Header("References")]
        [SerializeField]
        protected Projectiles.Projectile _projectile;
        
        [Header("Settings")]
        [SerializeField]
        protected TargetType _targetType;
        [SerializeField]
        protected int _amount = 1;
        [SerializeField]
        protected float _damage;
        protected bool _projectilesCanSpawnProjectiles = false;

        protected void InstantiateProjectile(GameObject owner, Vector3 position, Quaternion rotation)
        {
            Projectiles.Projectile projectile = Instantiate(_projectile, position, rotation);
            projectile.Instantiate(owner, _damage, _projectilesCanSpawnProjectiles);
        }
    } 
}
