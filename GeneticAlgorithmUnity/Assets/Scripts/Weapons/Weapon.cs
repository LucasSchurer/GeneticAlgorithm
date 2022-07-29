using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum Type 
    {   Rifle,
        Sniper,
        Count 
    }

    public Entity owner;

    [SerializeField]
    protected float _rateOfFire;
    protected float _rateOfFireTimer;

    [SerializeField]
    protected int _maxAmmo;
    [SerializeField]
    protected int _currentAmmo;

    [SerializeField]
    protected float _reloadTime;
    protected bool _isReloading = false;

    [SerializeField]
    protected Projectile.ProjectileData _projectileData;

    public Projectile.ProjectileData ProjectileData => _projectileData;

    /// <summary>
    /// Should be called after the component is instantiated.
    /// </summary>
    /// <param name="owner"></param>
    public virtual void Initialize(Entity owner)
    {
        this.owner = owner;
    }

    protected virtual void Update()
    {
        if (_rateOfFireTimer >= 0)
        {
            _rateOfFireTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Use a weapon, usually creating a projectile to a direction.
    /// </summary>
    /// <param name="direction"></param>
    public abstract void Use(Vector2 direction);

    protected virtual bool CanShoot()
    {
        return _rateOfFireTimer <= 0 && !_isReloading;
    }

    /// <summary>
    /// Use to register to the owner's events. Is called on 
    /// OnEnable()
    /// </summary>
    protected abstract void RegisterToOwnerEvents();

    /// <summary>
    /// Use to unregister to the owner's events. Is called on 
    /// OnDisable()
    /// </summary>
    protected abstract void UnregisterToOwnerEvents();

    protected virtual void OnEnable()
    {
        RegisterToOwnerEvents();
    }

    protected virtual void OnDisable()
    {
        UnregisterToOwnerEvents();
    }

    protected IEnumerator Reload()
    {
        _isReloading = true;

        yield return new WaitForSeconds(_reloadTime);

        _isReloading = false;
        _currentAmmo = _maxAmmo;
    }

    protected void ReduceAmmo()
    {
        _currentAmmo--;

        if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }
}
