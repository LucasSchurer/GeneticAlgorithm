using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected ScriptableObjects.Projectile _settings;
        
        private float _damage;
        private GameObject _owner;
        protected ProjectileEventController _eventController;

        protected virtual void Awake()
        {
            if (_settings.timeToDespawn > 0)
            {
                Destroy(gameObject, _settings.timeToDespawn);
            }
        }

        public void Instantiate(GameObject owner, float damage)
        {
            _owner = owner;
            _damage = damage;
            _eventController = GetComponent<ProjectileEventController>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!_owner.Equals(other.gameObject))
            {
                _eventController?.TriggerEvent(ProjectileEventType.OnHit, new ProjectileEventContext());
                other.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { owner = _owner, target = other.gameObject, healthModifier = -_damage });
            }
        }
    } 
}
