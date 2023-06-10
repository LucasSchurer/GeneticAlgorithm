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
            if (_canUse)
            {
                ctx.Weapon = new EntityEventContext.WeaponPacket() { RecoilStrength = _data.RecoilStrength };
                ctx.EventController.TriggerEvent(EntityEventType.OnWeaponAttack, ctx);

                _canUse = false;
                StartCoroutine(SpawnCoroutine());
            }
        }

        protected IEnumerator SpawnCoroutine()
        {
            yield return StartCoroutine(_data.OrbSpawn.Spawn(this, _data));

            StartCoroutine(Recharge());
        }

        protected virtual IEnumerator Recharge()
        {
            yield return new WaitForSeconds(_data.Cooldown);

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
