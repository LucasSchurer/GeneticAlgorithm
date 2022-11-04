using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Projectiles;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "SpawnProjectileAroundEntity", menuName = "Traits/Effects/Spawn Projectile Around Entity")]
    public class SpawnProjectileAroundEntity : Effect<EntityEventContext>
    {
        [SerializeField]
        private Projectile _bullet;
        [SerializeField]
        private float _damage = 1;
        [SerializeField]
        private int _amount = 1;
        [SerializeField]
        private TargetType _targetType;

        public override void Trigger(ref EntityEventContext ctx)
        {
            if (ctx.owner && EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                for (int i = 0; i < _amount; i++)
                {
                    float rotationY = (360 / _amount) * i;

                    Projectile projectile = Instantiate(_bullet, target.transform.position, Quaternion.Euler(0, rotationY, 0));
                    projectile.Instantiate(ctx.owner, _damage);
                }
            }
        }
    } 
}