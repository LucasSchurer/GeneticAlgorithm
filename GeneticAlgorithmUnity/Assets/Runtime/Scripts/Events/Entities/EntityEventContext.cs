using Game.Weapons;
using UnityEngine;

namespace Game.Events
{
    public class EntityEventContext : EventContext
    {
        private EntityEventController _eventController;

        private MovementPacket _movementPacket;
        private WeaponPacket _weaponPacket;
        private DamagePacket _damagePacket;
        private HealingPacket _healingPacket;
        private HealthChangePacket _healthChangePacket;

        private float _healthModifier;

        public EntityEventController EventController { get => _eventController; set => _eventController = value; }
        public MovementPacket Movement { get => _movementPacket; set => _movementPacket = value; }
        public WeaponPacket Weapon { get => _weaponPacket; set => _weaponPacket = value; }
        public DamagePacket Damage { get => _damagePacket; set => _damagePacket = value; }
        public HealingPacket Healing { get => _healingPacket; set => _healingPacket = value; }
        public HealthChangePacket HealthChange { get => _healthChangePacket; set => _healthChangePacket = value; }

        public float HealthModifier { get => _healthModifier; set => _healthModifier = value; }

        public class MovementPacket
        {
            private bool _isMoving = false;
            private Vector3 _movingDirection = Vector3.zero;
            private Vector3 _lookDirection = Vector3.zero;
            private float _movementAimPenalty = 0f;

            public bool IsMoving { get => _isMoving; set => _isMoving = value; }
            public Vector3 MovingDirection { get => _movingDirection; set => _movingDirection = value; }
            public Vector3 LookDirection { get => _lookDirection; set => _lookDirection = value; }
            public float MovementAimPenalty { get => _movementAimPenalty; set => _movementAimPenalty = value; }
        }

        public class WeaponPacket
        {
            private WeaponType _currentWeapon = WeaponType.None;
            private float _swapCooldown = 0f;

            private float _recoilStrength = 0f;

            public float RecoilStrength { get => _recoilStrength; set => _recoilStrength = value; }
            public WeaponType CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }
            public float SwapCooldown { get => _swapCooldown; set => _swapCooldown = value; }
        }

        public class DamagePacket
        {
            private float _damage = 0f;
            private DamageType _damageType = DamageType.None;
            private Vector3 _impactPoint = Vector3.zero;
            private Vector3 _hitDirection = Vector3.zero;

            public float Damage { get => _damage; set => _damage = value; }
            public DamageType DamageType { get => _damageType; set => _damageType = value; }
            public Vector3 ImpactPoint { get => _impactPoint; set => _impactPoint = value; }
            public Vector3 HitDirection { get => _hitDirection; set => _hitDirection = value; }
        }

        public class HealingPacket
        {
            private float _healing = 0f;
            private HealingType _healingType = HealingType.None;

            public float Healing { get => _healing; set => _healing = value; }
            public HealingType HealingType { get => _healingType; set => _healingType = value; }
        }
        public class HealthChangePacket
        {
            private float _maxHealth = -1f;
            private float _currentHealth = -1f;

            public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
            public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
        }

    }
}
