using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    [SerializeField]
    private Transform barrel;

    [SerializeField]
    private float _lockTime = 0.1f;
    private bool _isLockingOnTarget = false;
    private Vector3 _lockedTargetPosition;

    [SerializeField]
    private float _recoilStrength = 4f;

    private LineRenderer _lineRenderer;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Use(Vector2 direction)
    {
        if (!CanShoot() || _isLockingOnTarget)
        {
            return;
        }

        _isLockingOnTarget = true;

        StartCoroutine(LockAndFire());
    }

    private IEnumerator LockAndFire()
    {
        _lockedTargetPosition = barrel.position + (transform.forward * 20);
        owner.canMove = false;

        yield return new WaitForSeconds(_lockTime);

        ProjectileManager.Instance.SpawnProjectile(this, barrel, Projectile.Type.Bullet);

        owner.onWeaponFired?.Invoke();

        yield return new WaitForSeconds(0.1f);

        _rateOfFireTimer = _rateOfFire;
        ReduceAmmo();

        owner.Knockback(Vector3.back, _recoilStrength);
        _isLockingOnTarget = false;
        owner.canMove = true;
    }

    protected override void Update()
    {
        base.Update();

        DrawLine();
    }

    protected override void RegisterToOwnerEvents()
    {
        
    }

    protected override void UnregisterToOwnerEvents()
    {
        
    }

    private void DrawLine()
    {
        if (_lineRenderer != null)
        {
            if (_isLockingOnTarget)
            {
                _lineRenderer.SetPosition(1, _lockedTargetPosition);
            } else
            {
                _lineRenderer.SetPosition(1, barrel.position + (transform.forward * 20));
            }

            _lineRenderer.SetPosition(0, barrel.position);
        }
    }
}
