using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Projectiles;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "SpawnProjectileTowardsEntity", menuName = "Traits/Effects/Spawn Projecitle Towards Entity")]
    public class SpawnProjectileTowardsEntity : Effect<EntityEventContext>
    {
        [SerializeField]
        private Bullet _bullet;
        [SerializeField]
        private float _damage = 1;
        [SerializeField]
        private int _amount = 1;
        [SerializeField]
        private float _distance = 10f;
        [SerializeField]
        private TargetType _targetType;

        public override void Trigger(ref EntityEventContext ctx)
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


                    Projectile projectile = Instantiate(_bullet, position, Quaternion.LookRotation(-direction, Vector3.up));
                    projectile.Instantiate(ctx.owner, _damage, false);
                }
            }
        }
    } 
}
