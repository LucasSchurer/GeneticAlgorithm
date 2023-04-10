using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public abstract class SpawnProjectilesAroundTarget<Context> : SpawnProjectileEffect<Context>
        where Context: EventContext
    {
        public override void Trigger(ref Context ctx, int currentStacks = 1)
        {
            if (ctx.Owner && EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                int amount = _amount * currentStacks;

                for (int i = 0; i < amount; i++)
                {
                    float rotationY = (360 / amount) * i;

                    InstantiateProjectile(ctx.Owner, target.transform.position, Quaternion.Euler(0, rotationY, 0));
                }
            }
        }
    } 
}