using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public class CreatureDeathController : DeathController
    {
        [Header("References")]
        [SerializeField]
        private Rigidbody[] _bodyParts;

        [Header("Settings")]
        [SerializeField]
        private Vector2 _explosionRadiusInterval;
        [SerializeField]
        private float _explosionForce = 5;

        protected override void OnDeath(ref EntityEventContext ctx)
        {
            PlayDeathPhysicAnimation(ctx.agent != null ? ctx.agent.transform.position : transform.position);

            foreach (AI.AIBehaviour behaviour in GetComponents<Game.Entities.AI.AIBehaviour>())
            {
                behaviour.enabled = false;
            }
        }

        private void PlayDeathPhysicAnimation(Vector3 impactPoint)
        {
            float explosionForce;
            float explosionRadius;

            explosionForce = _explosionForce;
            explosionRadius = Random.Range(_explosionRadiusInterval.x, _explosionRadiusInterval.y);

            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            foreach (Rigidbody rb in _bodyParts)
            {
                rb.isKinematic = false;
                rb.GetComponent<Collider>().enabled = true;
                rb.AddExplosionForce(explosionForce, impactPoint, explosionRadius);
                rb.gameObject.layer = LayerMask.NameToLayer("Nothing");
            }

            gameObject.layer = LayerMask.NameToLayer("Nothing");
        }
    } 
}