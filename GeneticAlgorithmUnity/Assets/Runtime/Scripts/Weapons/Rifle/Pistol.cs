using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class Pistol : Weapon<HitscanWeaponData>
    {
        [SerializeField]
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

        private void Fire(ref EntityEventContext ctx)
        {
            if (_canUse)
            {
                _shootingParticleSystem?.Play();

                Vector3 trailEndPosition;

                if (Physics.Raycast(_weaponFireSocket.position, ctx.Direction, out RaycastHit hit, _data.HitRange, _hitLayer))
                {
                    trailEndPosition = hit.point;

                    EntityEventController other = hit.transform.GetComponent<EntityEventController>();

                    if (other != null) 
                    {
                        other.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = transform.gameObject, HealthModifier = -_data.Damage });
                        _eventController.TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, HealthModifier = -_data.Damage });
                    }

                    Instantiate(_data.OnHitParticle, hit.point, Quaternion.identity)?.Play();
                } else
                {
                    trailEndPosition = _weaponFireSocket.position + ctx.Direction * _data.HitRange;
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
