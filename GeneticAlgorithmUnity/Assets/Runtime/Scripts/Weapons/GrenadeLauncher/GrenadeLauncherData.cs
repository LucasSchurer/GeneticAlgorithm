using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/GrenadeLauncherData")]
    public class GrenadeLauncherData : WeaponData
    {
        [Header("Grenade Settings")]
        [SerializeField]
        private Projectiles.Grenade _grenade;
        [SerializeField]
        private float _launchStrength;
        [SerializeField]
        private float _explosionRadius;
        [SerializeField]
        private float _explosionTimer;
        [SerializeField]
        private Color _baseColor;
        [SerializeField]
        private int _grenadeLayer;

        public Projectiles.Grenade Grenade => _grenade;
        public float LaunchStrength => _launchStrength;
        public float ExplosionRadius => _explosionRadius;
        public float ExplosionTimer => _explosionTimer;
        public Color BaseColor => _baseColor;
        public int GrenadeLayer => _grenadeLayer;
    }
}

