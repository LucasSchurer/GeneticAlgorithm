using Game.Events;
using Game.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class HomingBots : Weapon<HomingBotsData>
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

                Vector3 spawnPosition = transform.position;

                spawnPosition.x += Random.Range(_data.XSpawnRange.x, _data.XSpawnRange.y);
                spawnPosition.y += Random.Range(_data.YSpawnRange.x, _data.YSpawnRange.y);
                spawnPosition.z += Random.Range(_data.ZSpawnRange.x, _data.ZSpawnRange.y);

                HomingBot homingBot = Instantiate(_data.HomingBot, spawnPosition, Quaternion.LookRotation(ctx.Movement.LookDirection));

                homingBot.Initialize(ctx.Owner, _data, this, _hitLayer, _entity.EnemyLayer);

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
    } 
}
