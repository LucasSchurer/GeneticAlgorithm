using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    private void Update()
    {
        transform.Translate(Vector3.forward * _data.speed * Time.deltaTime);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (((_obstacleLayer.value | _entityLayer.value) & (1 << collision.transform.gameObject.layer)) > 0)
        {
            if (collision.gameObject.tag == "Obstacle")
            {
                Destroy(gameObject);
            } else
            {
                Entity entity = collision.gameObject.GetComponent<Entity>();
                if (entity != null)
                {
                    HitEntity(entity);
                    _owner.hitCount++;
                }
            }
        }
    }
}
