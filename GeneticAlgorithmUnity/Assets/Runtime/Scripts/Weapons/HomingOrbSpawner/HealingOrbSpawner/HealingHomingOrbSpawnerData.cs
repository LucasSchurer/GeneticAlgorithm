using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HealingHomingOrbSpawnerData")]
    public class HealingHomingOrbSpawnerData : HomingOrbSpawnerData
    {
        [SerializeField]
        private float _yDistance;
        [SerializeField]
        private float _amount;
        [SerializeField]
        private float _orbSpawnRadius;
        [SerializeField]
        private float _orbSpawnInterval;

        public float YDistance => _yDistance;
        public float Amount => _amount;
        public float OrbSpawnRadius => _orbSpawnRadius;
        public float OrbSpawnInterval => _orbSpawnInterval;
    }
}

