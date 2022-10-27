/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : WeaponOld
{
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

    public override void Fire()
    {
        if (!CanShoot())
        {
            return;
        }

        _isLockingOnTarget = true;

        StartCoroutine(LockAndFire());
    }

    protected override bool CanShoot()
    {
        return base.CanShoot() && !_isLockingOnTarget;
    }

    private IEnumerator LockAndFire()
    {
        _lockedTargetPosition = _barrel.position + (transform.forward * 20);
        owner.canMove = false;

        yield return new WaitForSeconds(_lockTime);

        ProjectileManager.Instance.SpawnProjectile(this, _barrel, ProjectileOld.Type.Bullet);

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

    private void DrawLine()
    {
        if (_lineRenderer != null)
        {
            if (_isLockingOnTarget)
            {
                _lineRenderer.SetPosition(1, _lockedTargetPosition);
            } else
            {
                _lineRenderer.SetPosition(1, _barrel.position + (transform.forward * 20));
            }

            _lineRenderer.SetPosition(0, _barrel.position);
        }
    }
}
*/