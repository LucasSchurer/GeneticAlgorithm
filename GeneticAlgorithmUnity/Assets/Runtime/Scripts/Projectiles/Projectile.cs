using UnityEngine;
using Game.Events;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected ProjectileData _data;
        
        private float _damage;
        private GameObject _owner;
        protected ProjectileEventController _eventController;
        protected bool _canTriggerOnHitDealt;

        protected virtual void Awake()
        {
            if (_data.timeToDespawn > 0)
            {
                Destroy(gameObject, _data.timeToDespawn);
            }
        }

        public void Instantiate(GameObject owner, float damage, bool canTriggerOnHitDealt = true)
        {
            _owner = owner;
            _damage = damage;
            _eventController = GetComponent<ProjectileEventController>();
            _canTriggerOnHitDealt = canTriggerOnHitDealt;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!_owner)
            {
                Destroy(gameObject);
                return;
            }

            if (!_owner.Equals(other.gameObject))
            {
                _eventController?.TriggerEvent(ProjectileEventType.OnHit, new ProjectileEventContext());

                EntityEventController hitEventController = other.GetComponent<EntityEventController>();

                if (hitEventController)
                {
                    hitEventController.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { owner = other.gameObject, other = _owner, agent = gameObject, healthModifier = -_damage });

                    if (_canTriggerOnHitDealt)
                    {
                        _owner.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext { other = other.gameObject, agent = gameObject, healthModifier = -_damage });
                    }
                }

                Destroy(gameObject);
            }
        }
    } 
}
