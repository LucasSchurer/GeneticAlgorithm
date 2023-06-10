using Game.Events;
using Game.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public abstract class HomingOrbSpawner<Data> : Weapon<Data> 
        where Data : HomingOrbSpawnerData
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

        protected abstract void Fire(ref EntityEventContext ctx);

        protected virtual HomingOrb InstantiateAndInitializeHomingOrb(HomingOrb orbPrefab, LayerMask entityLayer, Vector3 spawnPosition, Quaternion rotation)
        {
            HomingOrb orb = Instantiate(orbPrefab, spawnPosition, rotation);

            orb.Initialize(gameObject, _data, _hitLayer, entityLayer);

            return orb;
        }

        protected virtual IEnumerator Recharge()
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

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
