using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOld : ProjectileOld
{
    private void Update()
    {
        transform.Translate(Vector3.forward * _data.speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((owner.obstacleLayerMask.value | owner.enemyLayerMask.value | owner.selfLayerMask) & (1 << collision.transform.gameObject.layer)) > 0)
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
