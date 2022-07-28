using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public override void Use(Vector2 direction)
    {
        if (!CanShoot())
        {
            return;
        }

        ProjectileManager.Instance.SpawnProjectile(this, Projectile.Type.Basic, direction);

        _rateOfFireTimer = _rateOfFire;
    }

    protected override void RegisterToOwnerEvents()
    {
        
    }

    protected override void UnregisterToOwnerEvents()
    {
        
    }
}
