using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Projectiles;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "SpawnProjectileAroundTarget", menuName = "Traits/Effects/Spawn Projectile Around Target")]
    public class SpawnProjectilesAroundTarget : SpawnProjectileEffect<EventContext>
    {
        public override void Trigger(ref EventContext ctx)
        {
            if (ctx.owner && EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                for (int i = 0; i < _amount; i++)
                {
                    float rotationY = (360 / _amount) * i;

                    InstantiateProjectile(ctx.owner, target.transform.position, Quaternion.Euler(0, rotationY, 0));
                }
            }
        }
    } 
}