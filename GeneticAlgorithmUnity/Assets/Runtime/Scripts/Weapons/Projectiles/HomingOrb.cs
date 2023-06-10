using Game.Events;
using Game.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class HomingOrb : PhysicalProjectile
    {
        [SerializeField]
        private float _collisionRadius;

        private HomingOrbSpawnerData _data;
        private DamageHomingOrbSpawner _weapon;
        private LayerMask _entityLayer;

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

        public void Initialize(GameObject owner, HomingOrbSpawnerData data, LayerMask collisionLayer, LayerMask entityLayer)
        {
            _data = data;
            _entityLayer = entityLayer;
            _collisionLayer = collisionLayer;
            _owner = owner;
            _damage = _data.Damage;

            _hitParticlesRenderer.material = _data.HitMaterial;

            _trailRenderer = GetComponent<TrailRenderer>();
            _trailRenderer.material = _data.TrailMaterial;

            _renderer = GetComponent<Renderer>();
            _renderer.material = _data.OrbMaterial;

            StartCoroutine(LockTargetCoroutine());
        }

        private IEnumerator LockTargetCoroutine()
        {
            yield return new WaitForSeconds(_data.StartLockingTime);

            Collider[] hits = Physics.OverlapSphere(transform.position, _data.FindTargetRadius, _entityLayer);

            if (hits.Length > 0)
            {
                List<Transform> validObjects = new List<Transform>();

                for (int i = 0; i < hits.Length; i++)
                {
                    if (_owner != null && hits[i].transform == _owner.transform)
                    {
                        continue;
                    }

                    validObjects.Add(hits[i].transform);
                }

                if (validObjects.Count > 0)
                {
                    _target = validObjects[Random.Range(0, validObjects.Count - 1)];
                    _directionToTarget = (_target.position - transform.position).normalized;

                    yield return new WaitForSeconds(_data.StartSeekingTime);

                    _isSeeking = true;
                } else
                {
                    DestroyOrb();
                }
            } else
            {
                DestroyOrb();
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

                    transform.position += _directionToTarget * _data.OrbSpeed * Time.deltaTime;
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
                        DamageType = Events.DamageType.Default,
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
                DestroyOrb();
            }
        }

        private void DestroyOrb()
        {
            _hitParticles.Play();

            _renderer.enabled = false;
            _trailRenderer.enabled = false;

            _canUpdate = false;
            Destroy(gameObject, _hitParticles.main.duration);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    } 
}
