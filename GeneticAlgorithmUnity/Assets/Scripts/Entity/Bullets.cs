using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu]
    public class Bullets : Trait<EntityEventType, EntityEventContext>
    {
        [SerializeField]
        private Projectiles.Projectile _projectile;
        [SerializeField]
        private int _quantity;
        [SerializeField]
        private float _cooldown;

        public override void Action(ref EntityEventContext ctx)
        {
            for (int i = 0; i < _quantity; i++)
            {
                Projectiles.Projectile projectile = Instantiate(_projectile, ctx.owner.transform.position, ctx.owner.transform.rotation);
                projectile.Instantiate(ctx.owner, 1);
            }
        }
    }
}
