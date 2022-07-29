using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField]
    private float _bulletDirectionVariation = 10f;

    public override void Fire()
    {
        if (!CanShoot())
        {
            return;
        }

        float yAngle = Random.Range(_barrel.rotation.eulerAngles.y - _bulletDirectionVariation, _barrel.rotation.eulerAngles.y + _bulletDirectionVariation);

        Quaternion randomBulletDirection = Quaternion.Euler(_barrel.rotation.eulerAngles.x, yAngle, _barrel.rotation.eulerAngles.z);

        ProjectileManager.Instance.SpawnProjectile(this, _barrel, Projectile.Type.Bullet, randomBulletDirection);
        owner.onWeaponFired?.Invoke();

        _rateOfFireTimer = _rateOfFire;
        ReduceAmmo();
    }
}
