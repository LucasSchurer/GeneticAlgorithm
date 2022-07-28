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
    public float damage;

    [SerializeField]
    protected float _rateOfFire;
    protected float _rateOfFireTimer;

    /// <summary>
    /// Should be called after the component is instantiated.
    /// </summary>
    /// <param name="owner"></param>
    public virtual void Initialize(Entity owner)
    {
        this.owner = owner;
    }

    private void Update()
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
        return _rateOfFireTimer <= 0;
    }

    protected virtual void UpdateRateOfFireTimer()
    {
        if (_rateOfFireTimer > 0)
        {
            _rateOfFireTimer -= Time.deltaTime;
        }
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
}
