using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/PhysicalProjectileLauncherData")]
    public class PhysicalProjectileLauncherData : WeaponData
    {
        [SerializeField]
        private Projectiles.PhysicalProjectile _physicalProjectile;
        [SerializeField]
        private float _launchStrength;

        public Projectiles.PhysicalProjectile PhysicalProjectile => _physicalProjectile;
        public float LaunchStrength => _launchStrength;
    }
}

