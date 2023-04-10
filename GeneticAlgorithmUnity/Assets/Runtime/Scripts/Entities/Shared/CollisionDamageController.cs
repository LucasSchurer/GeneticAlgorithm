using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class CollisionDamageController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _collisionMask;
        [SerializeField]
        private float _collisionRadius;

        private Collider[] hits = new Collider[1];
        private Collider hitCollider;

        private EntityEventController _eventController;
        private NonPersistentAttribute _collisionDamage;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
        }

        private void Start()
        {
            AttributeController attributeController = GetComponent<AttributeController>();
            if (attributeController != null)
            {
                _collisionDamage = attributeController.GetNonPersistentAttribute(AttributeType.CollisionDamage);
            }
            else
            {
                _collisionDamage = new NonPersistentAttribute();
            }
        }

        private void Update()
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(transform.position, _collisionRadius, hits, _collisionMask);

            if (hitsCount > 0)
            {
                hitCollider = hits[0];

                DoCollisionDamage();
            }
        }

        private void DoCollisionDamage()
        {
            if (hitCollider != null)
            {
                _eventController.TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Owner = gameObject, Other = hitCollider.gameObject, HealthModifier = -_collisionDamage.CurrentValue });
                hitCollider.gameObject.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Owner = hitCollider.gameObject, Other = gameObject, HealthModifier = -_collisionDamage.CurrentValue });
                hitCollider = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _collisionRadius);
        }
    } 
}
