using Game.Entities.Shared;
using Game.Events;
using System.Collections;
using UnityEngine;

namespace Game.Weapons
{
    public class ShieldDeployer : Weapon<ShieldDeployerData>
    {
        private Transform _weaponFireSocket;

        protected override void Awake()
        {
            base.Awake();
            _canUse = true;
        }

        protected override void SetSocketsAndVFXs()
        {
            _weaponFireSocket = _socketController.GetSocket(EntitySocketType.WeaponFire);
        }

        private void Fire(ref EntityEventContext ctx)
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

                Vector3 position = _weaponFireSocket.position;

                position += _weaponFireSocket.rotation * Vector3.forward * 1.5f;
                position.y += 0.5f;

                Shield shield = Instantiate(_data.Shield, position, _weaponFireSocket.rotation);
                shield.Initialize(_data);

                /*shield.Rigidbody.AddForce(ctx.Movement.LookDirection * _data.LaunchStrength, ForceMode.Impulse);*/

                StartCoroutine(Recharge());
            }
        }

        private IEnumerator Recharge()
        {
            _canUse = false;

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

        protected override void SetLayers() { }
    } 
}
