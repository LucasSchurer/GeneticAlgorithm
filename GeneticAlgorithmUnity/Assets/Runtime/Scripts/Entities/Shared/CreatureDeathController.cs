using Game.Events;
using UnityEngine;

namespace Game.Entities.Shared
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
            PlayDeathPhysicAnimation(ctx.Owner != null ? ctx.Owner.transform.position : transform.position);
        }

        private void PlayDeathPhysicAnimation(Vector3 impactPoint)
        {
            float explosionForce;
            float explosionRadius;

            explosionForce = _explosionForce;
            explosionRadius = Random.Range(_explosionRadiusInterval.x, _explosionRadiusInterval.y);

            foreach (Rigidbody rb in _bodyParts)
            {
                rb.transform.SetParent(null);
                rb.isKinematic = false;
                rb.GetComponent<Collider>().enabled = true;
                rb.AddExplosionForce(explosionForce, transform.position + new Vector3(Random.Range(-1f, 1f), 0f, (Random.Range(-1f, 1f))), explosionRadius);
                rb.gameObject.layer = LayerMask.NameToLayer("BodyPart");
            }

            gameObject.layer = LayerMask.NameToLayer("BodyPart");
        }
    } 
}
