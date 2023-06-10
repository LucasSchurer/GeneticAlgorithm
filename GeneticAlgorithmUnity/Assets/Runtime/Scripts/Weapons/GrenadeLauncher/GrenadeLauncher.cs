using Game.Events;
using Game.Projectiles;
using System.Collections;
using UnityEngine;

namespace Game.Weapons
{
    public class GrenadeLauncher : Weapon<GrenadeLauncherData>
    {
        public UnityEngine.Events.UnityAction explodeAll;

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

                Grenade grenade = Instantiate(_data.Grenade, _weaponFireSocket.position, transform.rotation);
                grenade.Initialize(this, _data.BaseColor, _data.Damage, _data.ExplosionRadius, _data.ExplosionTimer, _hitLayer, _entity.EnemyLayer, ctx.Owner);

                grenade.Rigidbody.AddForce(ctx.Movement.LookDirection * _data.LaunchStrength, ForceMode.Impulse);

                StartCoroutine(Recharge());
            }
        }

        private IEnumerator Recharge()
        {
            _canUse = false;

            yield return new WaitForSeconds(_data.Cooldown);

            _canUse = true;
        }

        private void ExplodeAllGrenades(ref EntityEventContext ctx)
        {
            explodeAll?.Invoke();
        }

        public override void StartListening()
        {
            base.StartListening();

            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnPrimaryActionPerformed, Fire);
                _eventController.AddListener(EntityEventType.OnSecondaryActionPerformed, ExplodeAllGrenades);
            }
        }

        public override void StopListening()
        {
            base.StopListening();

            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnPrimaryActionPerformed, Fire);
                _eventController.RemoveListener(EntityEventType.OnSecondaryActionPerformed, ExplodeAllGrenades);
            }
        }
    } 
}
