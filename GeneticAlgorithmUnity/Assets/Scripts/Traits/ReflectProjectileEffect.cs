using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Projectiles;

[CreateAssetMenu]
public class ReflectProjectileEffect : Effect<EntityEventContext>
{
    [SerializeField]
    private Bullet _bullet;
    [SerializeField]
    private float _damage = 1;
    [SerializeField]
    private int _amount = 1;

    public override void Trigger(ref EntityEventContext ctx)
    {
        for (int i = 0; i < _amount; i++)
        {
            float rotationY = (360 / _amount) * i;

            Projectile projectile = Instantiate(_bullet, ctx.owner.transform.position, Quaternion.Euler(0, rotationY, 0));
            projectile.Instantiate(ctx.owner, _damage);
        }
    }
}
