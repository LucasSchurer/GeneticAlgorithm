using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public enum Type
    {
        Basic,
        Fast,
        Count
    }

    public Weapon weapon;
    public Entity owner;

    [SerializeField]
    private LayerMask _entityLayer;
    [SerializeField]
    private LayerMask _obstacleLayer;
    
    [SerializeField]
    protected float _damage;
    [SerializeField]
    protected float _speed;
    protected Vector2 _direction;
    protected CircleCollider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
    }

    public void Initialize(Weapon weapon, Vector2 direction)
    {
        this.weapon = weapon;
        _direction = direction;
        transform.position = this.weapon.transform.position;
        owner = this.weapon.owner;
        _entityLayer = owner.enemyLayerMask;
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

    protected virtual void CheckCollision()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, _collider2D.radius, _obstacleLayer | _entityLayer);

        if (hitCollider != null)
        {
            if (hitCollider.gameObject.tag == "Obstacle")
            {
                Destroy(gameObject);
            } else
            {
                Entity hitEntity = hitCollider.gameObject.GetComponent<Entity>();

                if (hitEntity != null)
                {
                    HitEntity(hitEntity);
                }
            }
        }
    }

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
