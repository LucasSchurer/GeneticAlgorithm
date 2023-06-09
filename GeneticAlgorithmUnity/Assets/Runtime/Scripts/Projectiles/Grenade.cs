using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class Grenade : PhysicalProjectile
    {
        [SerializeField]
        private float _radius;
        [SerializeField]
        private float _explosionTime;
        [SerializeField]
        private float _intensityValue = 1f;
        [SerializeField]
        private float _minIntensity;
        [SerializeField]
        private float _maxIntensity;

        private Material _glowingEyeMaterial;

        public override void Initialize(float damage, LayerMask collisionLayer, LayerMask damageLayer, GameObject owner)
        {
            base.Initialize(damage, collisionLayer, damageLayer, owner);
            _glowingEyeMaterial = GetComponent<MeshRenderer>().materials[2];
            StartCoroutine(ExplosionCountdown());
        }

        private IEnumerator ExplosionCountdown()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _explosionTime)
            {
                _intensityValue = Mathf.Lerp(_minIntensity, _maxIntensity, elapsedTime / _explosionTime);

                _glowingEyeMaterial.SetFloat("_EmissionIntensity", _intensityValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Explode();
        }

        private void Explode()
        {
            foreach (Collider hit in Physics.OverlapSphere(transform.position, _radius, _damageLayer))
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
                    _owner.GetComponent<EntityEventController>().TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, Damage = damagePacket });
                }
            }

            Destroy(gameObject);
        }

        
    } 
}
