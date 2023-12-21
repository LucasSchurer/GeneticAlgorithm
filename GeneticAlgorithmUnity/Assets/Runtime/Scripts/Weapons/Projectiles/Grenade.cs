using Game.Events;
using Game.Weapons;
using System.Collections;
using UnityEngine;

namespace Game.Projectiles
{
    public class Grenade : PhysicalProjectile
    {
        [Header("References")]
        [SerializeField]
        private ParticleSystem _explosionParticles;
        [SerializeField]
        private Transform _explosionIndicator;

        [Header("Settings")]
        [SerializeField]
        private float _minIntensity;
        [SerializeField]
        private float _maxIntensity;
        
        private Material _glowingEyeMaterial;
        private float _explosionTime;
        private float _radius;
        private GrenadeLauncher _grenadeLauncher;

        private Coroutine _explosionCountdownCoroutine;

        public void Initialize(GrenadeLauncher grenadeLauncher, Color baseColor, float damage, float radius, float explosionTime, LayerMask collisionLayer, LayerMask damageLayer, GameObject owner)
        {
            _damage = damage;
            _radius = radius;
            _explosionTime = explosionTime;
            _collisionLayer = collisionLayer;
            _damageLayer = damageLayer;
            _owner = owner;
            _grenadeLauncher = grenadeLauncher;

            _grenadeLauncher.explodeAll += Explode;

            _rigidbody = GetComponent<Rigidbody>();

            _glowingEyeMaterial = GetComponent<MeshRenderer>().materials[2];
            _glowingEyeMaterial.SetColor("_EmissionColor", baseColor);

            ParticleSystem.MainModule main = _explosionParticles.main;
            main.startSize = _radius * 2f;

            _explosionIndicator.localScale = new Vector3(_radius * 2f, _radius * 2f, _radius * 2f);
            _explosionIndicator.GetComponent<MeshRenderer>().material.SetColor("_Color", baseColor);

            _explosionCountdownCoroutine = StartCoroutine(ExplosionCountdown());
        }

        private IEnumerator ExplosionCountdown()
        {
            float elapsedTime = 0f;
            float emissionIntensity = 1f;

            while (elapsedTime < _explosionTime)
            {
                emissionIntensity = Mathf.Lerp(_minIntensity, _maxIntensity, elapsedTime / _explosionTime);

                _glowingEyeMaterial.SetFloat("_EmissionIntensity", emissionIntensity);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Explode();
        }

        public void Explode()
        {
            if (_explosionCountdownCoroutine != null)
            {
                StopCoroutine(_explosionCountdownCoroutine);
            }

            _explosionParticles.Play();

            foreach (Collider hit in Physics.OverlapSphere(transform.position, _radius, _damageLayer))
            {
                EntityEventController other = hit.transform.GetComponent<EntityEventController>();

                if (other != null)
                {
                    EntityEventContext.DamagePacket damagePacket = new EntityEventContext.DamagePacket()
                    {
                        Damage = _damage,
                        DamageType = Events.DamageType.Explosive,
                        ImpactPoint = hit.transform.position,
                        HitDirection = (hit.transform.position - transform.position).normalized
                    };

                    other.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = transform.gameObject, Damage = damagePacket });

                    if (_owner)
                    {
                        _owner.GetComponent<EntityEventController>().TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, Damage = damagePacket });
                    }
                }
            }

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            _explosionIndicator.gameObject.SetActive(false);
            Destroy(gameObject, _explosionParticles.main.duration);
        }

        private void OnDestroy()
        {
            _grenadeLauncher.explodeAll -= Explode;
        }
    } 
}
