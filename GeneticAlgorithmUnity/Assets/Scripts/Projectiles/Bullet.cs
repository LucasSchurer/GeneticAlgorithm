using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    private void Update()
    {
        transform.Translate(Vector3.forward * _data.speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((_owner.obstacleLayerMask.value | _owner.enemyLayerMask.value | _owner.selfLayerMask) & (1 << collision.transform.gameObject.layer)) > 0)
        {
            if (collision.gameObject.tag == "Obstacle")
            {
                Destroy(gameObject);
            }
            else
            {
                Entity entity = collision.gameObject.GetComponent<Entity>();
                if (entity != null)
                {
                    HitEntity(entity);
                }
            }
        }
    }
}
