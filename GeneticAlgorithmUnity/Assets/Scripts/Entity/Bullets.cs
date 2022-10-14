using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu]
    public class Bullets : Trait<EntityEventType, EntityEventContext>
    {
        [SerializeField]
        private Projectiles.Projectile _projectile;
        [SerializeField]
        private int _quantity;

        public override void Action(ref EntityEventContext ctx)
        {
            for (int i = 0; i < _quantity; i++)
            {
                Instantiate(_projectile, Vector3.zero, Quaternion.identity);
            }
        }
    }
}
