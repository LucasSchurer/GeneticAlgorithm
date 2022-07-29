using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    private LineRenderer _lineRenderer;

    public Vector3 direction;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Use(Vector2 direction)
    {
        if (!CanShoot())
        {
            return;
        }

        ProjectileManager.Instance.SpawnProjectile(this, Projectile.Type.Bullet, transform.forward);

        _rateOfFireTimer = _rateOfFire;
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
        direction = transform.forward;

        if (_lineRenderer != null)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, transform.position + (transform.forward * 10));
        }
    }
}
