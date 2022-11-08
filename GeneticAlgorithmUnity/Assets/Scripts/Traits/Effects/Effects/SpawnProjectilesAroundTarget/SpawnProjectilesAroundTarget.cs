using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public abstract class SpawnProjectilesAroundTarget<Context> : SpawnProjectileEffect<Context>
        where Context: EventContext
    {
        public override void Trigger(ref Context ctx)
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