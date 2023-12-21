using Game.Entities.Shared;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class Nuke : Weapon<NukeData>
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

                Vector3 beaconSpawnPosition = Vector3.zero;

                if (_entity.IsPlayer)
                {
                    if (Physics.Raycast(_weaponFireSocket.position, ctx.Movement.LookDirection, out RaycastHit hit, Constants.GroundLayer))
                    {
                        beaconSpawnPosition = hit.point;
                    } else
                    {
                        return;
                    }
                } else
                {
                    Collider[] hits = Physics.OverlapSphere(transform.position, _data.NPCDetectionRange, _entity.EnemyLayer);

                    if (hits.Length > 0)
                    {
                        Collider hit = hits[Random.Range(0, hits.Length - 1)];

                        if (Physics.Raycast(hit.transform.position, Vector3.down, out RaycastHit groundHit, Constants.GroundLayer))
                        {
                            beaconSpawnPosition = groundHit.point;
                        } else
                        {
                            return;
                        }
                    } else
                    {
                        return;
                    }
                }

                beaconSpawnPosition.y += 0.3f;

                NukeBeacon beacon = Instantiate(_data.Beacon, beaconSpawnPosition, Quaternion.identity);
                beacon.Initialize(ctx.Owner, this, _data, _entity.EnemyLayer);

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
