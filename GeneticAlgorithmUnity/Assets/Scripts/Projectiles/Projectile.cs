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
        public Vector2 size;
        public Color color;
        public float range;
    }

    [SerializeField]
    protected LayerMask _obstacleLayer;

    protected Weapon _weapon;
    protected Entity _owner;

    protected ProjectileData _data;
    protected Vector2 _direction;
    protected LayerMask _entityLayer;
    protected SpriteRenderer _spriteRenderer;

    protected void Awake()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void Initialize(Weapon weapon, Vector2 direction)
    {
        _direction = direction;
        _weapon = weapon;
        _owner = _weapon.owner;
        _data = _weapon.ProjectileData;

        _entityLayer = _owner.enemyLayerMask;
        transform.localScale = _data.size;
        transform.position = _weapon.transform.position;

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _data.color;
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

    protected abstract void CheckCollision();

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
