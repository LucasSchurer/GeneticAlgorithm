using UnityEngine;

namespace Game.Projectiles
{
    [CreateAssetMenu]
    public class ProjectileData : ScriptableObject
    {
        public float speed;
        public float damage;
        public float timeToDespawn;
    } 
}
