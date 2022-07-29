using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField]
    private Transform barrel;

    [SerializeField]
    private float _bulletDirectionVariation = 10f;

    public override void Use(Vector2 direction)
    {
        if (!CanShoot())
        {
            return;
        }

        float yAngle = Random.Range(barrel.rotation.eulerAngles.y - _bulletDirectionVariation, barrel.rotation.eulerAngles.y + _bulletDirectionVariation);

        Quaternion randomBulletDirection = Quaternion.Euler(barrel.rotation.eulerAngles.x, yAngle, barrel.rotation.eulerAngles.z);

        ProjectileManager.Instance.SpawnProjectile(this, barrel, Projectile.Type.Bullet, randomBulletDirection);

        _rateOfFireTimer = _rateOfFire;

        ReduceAmmo();
    }

    protected override void RegisterToOwnerEvents()
    {
        
    }

    protected override void UnregisterToOwnerEvents()
    {
        
    }
}
