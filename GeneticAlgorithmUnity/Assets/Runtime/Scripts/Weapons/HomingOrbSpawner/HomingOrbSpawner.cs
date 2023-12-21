using Game.Entities;
using Game.Events;
using Game.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class HomingOrbSpawner : Weapon<HomingOrbSpawnerData> 
    {
        protected LayerMask _hitLayer;
        protected Transform _weaponFireSocket;

        private NonPersistentAttribute _projectileAmount;

        public Transform WeaponFireSocket => _weaponFireSocket;

        protected override void SetNonPersistentAttributes()
        {
            base.SetNonPersistentAttributes();

            if (_attributeController)
            {
                _projectileAmount = _attributeController.GetNonPersistentAttribute(AttributeType.ProjectileAmount);
            }
            else
            {
                _projectileAmount = _attributeController.GetNonPersistentAttribute(AttributeType.ProjectileAmount);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _canUse = true;
        }

        protected override void SetSocketsAndVFXs()
        {
            _weaponFireSocket = _socketController.GetSocket(Entities.Shared.EntitySocketType.WeaponFire);
        }

        protected virtual void Fire(ref EntityEventContext ctx)
        {
            if (_canUse && (!_data.UseAmmunition || _currentAmmunition > 0))
            {
                _currentAmmunition--;

                ctx.Weapon = new EntityEventContext.WeaponPacket()
                {
                    CurrentWeapon = _data.weaponType,
                    Cooldown = _data.Cooldown,
                    CurrentAmmunition = _currentAmmunition,
                    RecoilStrength = _data.RecoilStrength
                };

                ctx.EventController.TriggerEvent(EntityEventType.OnWeaponAttack, ctx);

                _canUse = false;
                StartCoroutine(SpawnCoroutine());
            }
        }

        protected IEnumerator SpawnCoroutine()
        {
            yield return StartCoroutine(_data.OrbSpawn.Spawn(this, _data, Mathf.FloorToInt(_projectileAmount.CurrentValue)));

            StartCoroutine(Recharge());
        }

        protected virtual IEnumerator Recharge()
        {
            yield return new WaitForSeconds(_data.Cooldown - (_data.Cooldown * _rateOfFireMultiplier.CurrentValue));

            _canUse = true;
        }

        public override void StartListening()
        {
            base.StartListening();

            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnPrimaryActionPerformed, Fire);
            }
        }

        public override void StopListening()
        {
            base.StopListening();

            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnPrimaryActionPerformed, Fire);
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        protected override void SetLayers() { }
    }
}
