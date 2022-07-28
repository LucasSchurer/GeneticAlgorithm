using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    private CircleCollider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        CheckCollision();
        transform.Translate(_direction * _data.speed * Time.deltaTime);
    }

    protected override void RegisterToOwnerEvents()
    {

    }

    protected override void UnregisterToOwnerEvents()
    {

    }

    protected override void HitEntity(Entity entity)
    {
        entity.Damage(_owner, _data.damage);
        Destroy(gameObject);
    }

    protected override void CheckCollision()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, _collider2D.radius, _obstacleLayer | _entityLayer);

        if (hitCollider != null)
        {
            if (hitCollider.gameObject.tag == "Obstacle")
            {
                Destroy(gameObject);
            }
            else
            {
                Entity hitEntity = hitCollider.gameObject.GetComponent<Entity>();

                if (hitEntity != null)
                {
                    HitEntity(hitEntity);
                }
            }
        }
    }
}
