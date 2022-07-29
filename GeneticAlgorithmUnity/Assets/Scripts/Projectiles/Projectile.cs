using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public enum Type
    {
        Bullet,
        Count
    }

    [System.Serializable]
    public struct ProjectileData
    {
        public float speed;
        public float damage;
        public Vector3 size;
        public Color color;
        public float range;
    }

    [SerializeField]
    protected LayerMask _obstacleLayer;

    protected Weapon _weapon;
    protected Entity _owner;

    protected Rigidbody _rb;
    protected ProjectileData _data;
    [SerializeField]
    protected LayerMask _entityLayer;

    protected void Awake()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
    }

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
        _owner = _weapon.owner;
        _data = _weapon.ProjectileData;

        transform.localScale = _data.size;

        _entityLayer = _owner.enemyLayerMask;
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

    protected abstract void HitEntity(Entity entity);

    protected virtual void OnEnable()
    {
        RegisterToOwnerEvents();
    }

    protected virtual void OnDisable()
    {
        UnregisterToOwnerEvents();
    }
}
