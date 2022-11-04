using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Projectiles;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "SpawnProjectileTowardsTarget", menuName = "Traits/Effects/Spawn Projecitle Towards Target")]
    public class SpawnProjectilesTowardsTarget : SpawnProjectileEffect<EventContext>
    {
        [SerializeField]
        private float _distance = 10f;

        public override void Trigger(ref EventContext ctx)
        {
            if (ctx.owner && EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                for (int i = 0; i < _amount; i++)
                {
                    Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

                    if (direction == Vector3.zero)
                    {
                        direction = Vector3.forward;
                    }

                    Vector3 position = new Vector3(target.transform.position.x + direction.x * _distance, target.transform.position.y, ctx.other.transform.position.z + direction.z * _distance);


                    Projectile projectile = Instantiate(_projectile, position, Quaternion.LookRotation(-direction, Vector3.up));
                    projectile.Instantiate(ctx.owner, _damage, false);
                }
            }
        }
    } 
}
