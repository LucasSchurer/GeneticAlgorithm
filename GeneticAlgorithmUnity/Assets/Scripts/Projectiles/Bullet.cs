using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class Bullet : Projectile
    {
        private void Update()
        {
            transform.Translate(Vector3.forward * _settings.speed * Time.deltaTime);
        }
    } 
}
