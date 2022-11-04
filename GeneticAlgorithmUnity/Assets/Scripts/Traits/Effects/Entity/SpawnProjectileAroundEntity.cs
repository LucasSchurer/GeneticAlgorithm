using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Projectiles;

namespace Game.Traits.Effects
{
    public class SpawnProjectileAroundEntity : Effect<EntityEventContext>
    {
        private enum Target
        {
            Self,
            Other
        }

        [SerializeField]
        private Bullet _bullet;
        [SerializeField]
        private float _damage = 1;
        [SerializeField]
        private int _amount = 1;
        [SerializeField]
        private Target _target;

        public override void Trigger(ref EntityEventContext ctx)
        {
            GameObject target = GetTarget(ref ctx);

            if (target && ctx.owner)
            {
                for (int i = 0; i < _amount; i++)
                {
                    float rotationY = (360 / _amount) * i;

                    Projectile projectile = Instantiate(_bullet, target.transform.position, Quaternion.Euler(0, rotationY, 0));
                    projectile.Instantiate(ctx.owner, _damage);
                }
            }
        }

        private GameObject GetTarget(ref EntityEventContext ctx)
        {
            switch (_target)
            {
                case Target.Self:
                    return ctx.owner;
                case Target.Other:
                    return ctx.other;
                default:
                    return null;
            }
        }
    } 
}