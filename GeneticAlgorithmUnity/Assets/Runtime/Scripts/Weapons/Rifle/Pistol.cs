using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class Pistol : Weapon<HitscanWeaponData>
    {
        private LayerMask _hitLayer;
        private Transform _weaponFireSocket;
        private ParticleSystem _shootingParticleSystem;

        protected override void Awake()
        {
            base.Awake();
            _canUse = true;
        }

        protected override void SetSocketsAndVFXs()
        {
            _weaponFireSocket = _socketController.GetSocket(Entities.Shared.EntitySocketType.WeaponFire);

            if (_weaponFireSocket)
            {
                _shootingParticleSystem = Instantiate(_data.AttackParticle, _weaponFireSocket);
            }
        }

        protected override void SetLayers()
        {
            _hitLayer = (_entity.EnemyLayer | (1 << Constants.GroundLayer));
        }

        private void Fire(ref EntityEventContext ctx)
        {
            if (_canUse && (!_data.UseAmmunition | _currentAmmunition > 0))
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

                _shootingParticleSystem?.Play();

                Vector3 trailEndPosition;

                if (Physics.Raycast(_weaponFireSocket.position, ctx.Movement.LookDirection, out RaycastHit hit, _data.HitRange, _hitLayer))
                {
                    trailEndPosition = hit.point;

                    EntityEventController other = hit.transform.GetComponent<EntityEventController>();

                    if (other != null) 
                    {
                        EntityEventContext.DamagePacket damagePacket = new EntityEventContext.DamagePacket()
                        {
                            DamageType = Events.DamageType.Common,
                            Damage = _data.Damage,
                            ImpactPoint = hit.point,
                            HitDirection = ctx.Movement.LookDirection
                        };

                        other.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = transform.gameObject, Damage = damagePacket });
                        _eventController.TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, Damage = damagePacket });
                    }

                    Instantiate(_data.OnHitParticle, hit.point, Quaternion.identity)?.Play();
                } else
                {
                    trailEndPosition = _weaponFireSocket.position + ctx.Movement.LookDirection * _data.HitRange;
                }

                TrailRenderer trail = Instantiate(_data.AttackTrail, _weaponFireSocket.position, Quaternion.identity);
                StartCoroutine(SpawnBulletTrail(trail, trailEndPosition));

                StartCoroutine(Recharge());
            }
        }

        private IEnumerator SpawnBulletTrail(TrailRenderer trail, Vector3 hitPoint)
        {
            Vector3 startPosition = trail.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime <= _data.TrailSpeed)
            {
                elapsedTime += Time.deltaTime;
                trail.transform.position = Vector3.Lerp(startPosition, hitPoint, elapsedTime / _data.TrailSpeed);
                yield return null;
            }

            trail.transform.position = hitPoint;

            Destroy(trail.gameObject, trail.time);
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

            _eventController?.AddListener(EntityEventType.OnPrimaryActionPerformed, Fire);
        }

        public override void StopListening()
        {
            base.StopListening();

            _eventController?.RemoveListener(EntityEventType.OnPrimaryActionPerformed, Fire);
        }
    } 
}
