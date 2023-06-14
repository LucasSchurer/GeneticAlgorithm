using Game.Events;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class CreatureDeathController : DeathController
    {
        [Header("References")]
        [SerializeField]
        private Rigidbody[] _bodyParts;
        [SerializeField]
        private Transform _objectRoot;
        [SerializeField]
        private Transform _objectMesh;

        [Header("Settings")]
        [SerializeField]
        private Vector2 _explosionRadiusInterval;
        [SerializeField]
        private float _explosionForce = 5;
        [SerializeField]
        private float _bodyPartDespawnTime = 5f;

        protected override void OnDeath(ref EntityEventContext ctx)
        {
            DetachBodyParts(ctx.Damage);

            Destroy(_objectRoot.gameObject);
        }

        private void DetachBodyParts(EntityEventContext.DamagePacket damagePacket)
        {
            GameObject bodyParts = new GameObject("BotBodyParts");

            float explosionForce;
            float explosionRadius;
            Vector3 explosionPoint;

            explosionForce = _explosionForce;
            explosionRadius = Random.Range(_explosionRadiusInterval.x, _explosionRadiusInterval.y);
            
            if (damagePacket != null && damagePacket.ImpactPoint != Vector3.zero)
            {
                explosionPoint = damagePacket.ImpactPoint;
            } else
            {
                explosionPoint = transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            }


            foreach (Rigidbody rb in _bodyParts)
            {
                rb.transform.SetParent(bodyParts.transform);
                rb.isKinematic = false;
                rb.GetComponent<Collider>().enabled = true;
                rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
                rb.gameObject.layer = Constants.BodyPartLayer;
            }

            _objectMesh.SetParent(bodyParts.transform);

            Destroy(bodyParts, _bodyPartDespawnTime); 
        }
    } 
}
