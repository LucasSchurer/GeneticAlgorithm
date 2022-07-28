using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
    private void Update()
    {
        CheckCollision();
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    protected override void RegisterToOwnerEvents()
    {
        
    }

    protected override void UnregisterToOwnerEvents()
    {
        
    }

    protected override void HitEntity(Entity entity)
    {
        entity.Damage(owner, _damage + weapon.damage);
        Destroy(gameObject);
    }
}
