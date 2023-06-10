using Game.Events;
using Game.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class HomingBot : PhysicalProjectile
    {
        [SerializeField]
        private float _collisionRadius;

        private HomingBotsData _data;
        private HomingBots _weapon;
        private LayerMask _enemyLayer;

        private bool _isSeeking = false;
        private Transform _target;
        private Vector3 _directionToTarget;

        private bool _canUpdate = true;

        private TrailRenderer _trailRenderer;
        private Renderer _renderer;

        [SerializeField]
        private ParticleSystem _hitParticles;
        [SerializeField]
        private ParticleSystemRenderer _hitParticlesRenderer;

        public void Initialize(GameObject owner, HomingBotsData data, HomingBots weapon, LayerMask collisionLayer, LayerMask enemyLayer)
        {
            _data = data;
            _weapon = weapon;
            _enemyLayer = enemyLayer;
            _collisionLayer = collisionLayer;
            _owner = owner;
            _damage = _data.Damage;

            _hitParticlesRenderer.material = _data.BotHitMaterial;

            _trailRenderer = GetComponent<TrailRenderer>();
            _trailRenderer.material = _data.TrailMaterial;

            _renderer = GetComponent<Renderer>();
            _renderer.material = _data.BotMaterial;

            StartCoroutine(LockTargetCoroutine());
        }

        private IEnumerator LockTargetCoroutine()
        {
            yield return new WaitForSeconds(_data.TimeToStartLock);

            Collider[] hits = Physics.OverlapSphere(transform.position, _data.SeekRadius, _enemyLayer);

            if (hits.Length > 0)
            {
                Collider hit = hits[Random.Range(0, hits.Length - 1)];

                _target = hit.transform;

                _directionToTarget = (_target.position - transform.position).normalized;

                yield return new WaitForSeconds(_data.DelayToStartSeeking);

                _isSeeking = true;
            } else
            {
                DestroyBot();
            }
        }

        private void Update()
        {
            if (_canUpdate)
            {
                if (_isSeeking)
                {
                    if (_data.HardLock)
                    {
                        if (_target != null)
                        {
                            _directionToTarget = (_target.position - transform.position).normalized;
                        }
                    }

                    transform.position += _directionToTarget * _data.HomingBotSpeed * Time.deltaTime;
                }

                CheckCollisions();
            }
        }

        private void CheckCollisions()
        {
            bool hasHit = false;
            foreach (Collider hit in Physics.OverlapSphere(transform.position, _collisionRadius, _collisionLayer))
            {
                EntityEventController other = hit.transform.GetComponent<EntityEventController>();

                if (other != null)
                {
                    EntityEventContext.DamagePacket damagePacket = new EntityEventContext.DamagePacket()
                    {
                        Damage = _damage,
                        ImpactPoint = hit.transform.position,
                        HitDirection = (hit.transform.position - transform.position).normalized
                    };

                    other.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = transform.gameObject, Damage = damagePacket });
                    
                    if (_owner)
                    {
                        _owner.GetComponent<EntityEventController>().TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, Damage = damagePacket });
                    }
                }

                hasHit = true;
            }

            if (hasHit)
            {
                DestroyBot();
            }
        }

        private void DestroyBot()
        {
            _hitParticles.Play();

            _renderer.enabled = false;
            _trailRenderer.enabled = false;

            _canUpdate = false;
            Destroy(gameObject, _hitParticles.main.duration);
        }
    } 
}
