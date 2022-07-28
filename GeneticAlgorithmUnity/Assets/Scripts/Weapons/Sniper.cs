using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    public override void Use(Vector2 direction)
    {
        if (!CanShoot())
        {
            return;
        }

        ProjectileManager.Instance.SpawnProjectile(this, Projectile.Type.Bullet, direction);

        _rateOfFireTimer = _rateOfFire;
    }

    protected override void RegisterToOwnerEvents()
    {
        
    }

    protected override void UnregisterToOwnerEvents()
    {
        
    }
}
