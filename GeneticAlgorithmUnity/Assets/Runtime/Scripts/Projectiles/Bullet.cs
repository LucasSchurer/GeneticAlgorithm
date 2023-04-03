using UnityEngine;

namespace Game.Projectiles
{
    public class Bullet : Projectile
    {
        private void Update()
        {
            transform.Translate(Vector3.forward * _data.speed * Time.deltaTime);
        }
    } 
}
