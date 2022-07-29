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

    protected Weapon _weapon;
    protected Entity _owner;
    protected ProjectileData _data;

    protected LayerMask _selfLayerMask;
    protected LayerMask _enemyLayerMask;
    protected LayerMask _obstacleLayerMask;

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
        _owner = _weapon.owner;
        _data = _weapon.ProjectileData;

        transform.localScale = _data.size;
    }

    /// <summary>
    /// Invoke the onHit and whenHit events and destroy the projectile.
    /// </summary>
    /// <param name="entity"></param>
    protected virtual void HitEntity(Entity entity)
    {
        _owner.onHit?.Invoke(entity, _data.damage, this);
        entity.whenHit?.Invoke(_owner, _data.damage, this);
        Destroy(gameObject);        
    }
}
