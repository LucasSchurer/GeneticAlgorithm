using Game.Events;
using Game.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class PhysicalProjectileLauncher : Weapon<PhysicalProjectileLauncherData>
    {
        private LayerMask _hitLayer;
        private Transform _weaponFireSocket;

        protected override void Awake()
        {
            base.Awake();
            _canUse = true;
        }

        protected override void SetLayers()
        {
            _hitLayer = _entity.EnemyLayer | (1 << Constants.GroundLayer);
        }


        protected override void SetSocketsAndVFXs()
        {
            _weaponFireSocket = _socketController.GetSocket(Entities.Shared.EntitySocketType.WeaponFire);
        }

        private void Fire(ref EntityEventContext ctx)
        {
            if (_canUse)
            {
                ctx.Weapon = new EntityEventContext.WeaponPacket() { RecoilStrength = _data.RecoilStrength };
                ctx.EventController.TriggerEvent(EntityEventType.OnWeaponAttack, ctx);

                PhysicalProjectile physicalProjectile = Instantiate(_data.PhysicalProjectile, _weaponFireSocket.position, transform.rotation);
                physicalProjectile.Initialize(_data.Damage, _hitLayer, _entity.EnemyLayer, ctx.Owner);

                physicalProjectile.Rigidbody.AddForce(ctx.Movement.LookDirection * _data.LaunchStrength, ForceMode.Impulse);

                StartCoroutine(Recharge());
            }
        }

        private IEnumerator Recharge()
        {
            _canUse = false;

            yield return new WaitForSeconds(_data.Cooldown);

            _canUse = true;
        }

        public override void StartListening()
        {
            base.StartListening();

            _eventController?.AddListener(EntityEventType.OnPrimaryActionPerformed, Fire);
        }

        public override void StopListening()
        {
            base.StopListening();

            _eventController?.RemoveListener(EntityEventType.OnPrimaryActionPerformed, Fire);
        }
    } 
}
