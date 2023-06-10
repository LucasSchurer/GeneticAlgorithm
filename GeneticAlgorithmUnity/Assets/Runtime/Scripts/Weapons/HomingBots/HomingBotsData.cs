using UnityEngine;
using Game.Projectiles;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HomingBotsData")]
    public class HomingBotsData : WeaponData
    {
        [Header("Homing Bots Settings")]
        [SerializeField]
        private HomingBot _homingBot;
        [SerializeField]
        private float _homingBotSpeed;

        [SerializeField]
        private Vector2 _ySpawnRange;
        [SerializeField]
        private Vector2 _zSpawnRange;
        [SerializeField]
        private Vector2 _xSpawnRange;
        [SerializeField]
        private float _timeToStartLock;
        [SerializeField]
        private float _delayToStartSeeking;
        [SerializeField]
        private float _seekRadius;
        [SerializeField]
        [Tooltip("If checked, bots won't lock a position and will follow the target")]
        private bool _hardLock;
        [SerializeField]
        private Material _trailMaterial;
        [SerializeField]
        private Material _botMaterial;
        [SerializeField]
        private Material _botHitMaterial;

        public HomingBot HomingBot => _homingBot;

        public Vector2 YSpawnRange => _ySpawnRange;
        public Vector2 ZSpawnRange => _zSpawnRange;
        public Vector2 XSpawnRange => _xSpawnRange;
        public float TimeToStartLock => _timeToStartLock;
        public float DelayToStartSeeking => _delayToStartSeeking;
        public float SeekRadius => _seekRadius;
        public float HomingBotSpeed => _homingBotSpeed;
        public bool HardLock => _hardLock;
        public Material TrailMaterial => _trailMaterial;
        public Material BotMaterial => _botMaterial;
        public Material BotHitMaterial => _botHitMaterial;
    }
}

