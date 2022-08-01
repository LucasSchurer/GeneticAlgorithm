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

    public Weapon weapon;
    public Entity owner;
    protected ProjectileData _data;

    protected LayerMask _selfLayerMask;
    protected LayerMask _enemyLayerMask;
    protected LayerMask _obstacleLayerMask;

    public void Initialize(Weapon weapon)
    {
        this.weapon = weapon;
        owner = this.weapon.owner;
        _data = this.weapon.ProjectileData;

        transform.localScale = _data.size;
    }

    /// <summary>
    /// Invoke the onHit and whenHit events and destroy the projectile.
    /// </summary>
    /// <param name="entity"></param>
    protected virtual void HitEntity(Entity entity)
    {
        owner.onHit?.Invoke(entity, _data.damage, this);
        entity.whenHit?.Invoke(owner, _data.damage, this);
        Destroy(gameObject);        
    }
}
