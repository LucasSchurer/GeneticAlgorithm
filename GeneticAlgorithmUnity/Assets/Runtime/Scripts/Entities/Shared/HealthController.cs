using UnityEngine;
using Game.Events;

namespace Game.Entities.Shared
{
    public class HealthController : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private NonPersistentAttribute _health;

        private EntityEventController _eventController;
        private AttributeController _attributeController;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _attributeController = GetComponent<AttributeController>();
        }

        private void Start()
        {
            if (_attributeController)
            {
                _health = _attributeController.GetNonPersistentAttribute(AttributeType.Health);
            } else
            {
                _health = new NonPersistentAttribute();
            }
        }

        private void OnHitTaken(ref EntityEventContext ctx)
        {
            _health.CurrentValue += ctx.HealthModifier;

            if (_health.CurrentValue <= 0)
            {
                _eventController.TriggerEvent(EntityEventType.OnDeath, ctx);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }

        public void StartListening()
        {
            if (_eventController != null)
            {
                _eventController.AddListener(EntityEventType.OnHitTaken, OnHitTaken);
            }
        }

        public void StopListening()
        {
            if (_eventController != null)
            {
                _eventController.RemoveListener(EntityEventType.OnHitTaken, OnHitTaken);
            }
        }
    } 
}
