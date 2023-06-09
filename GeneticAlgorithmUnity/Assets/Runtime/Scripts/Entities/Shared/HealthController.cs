using UnityEngine;
using Game.Events;
using System.Collections;

namespace Game.Entities.Shared
{
    public class HealthController : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private NonPersistentAttribute _health;
        private NonPersistentAttribute _invulnerabilityTime;

        private EntityEventController _eventController;
        private AttributeController _attributeController;

        private bool _isInvulnerable = false;

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
                _invulnerabilityTime = _attributeController.GetNonPersistentAttribute(AttributeType.InvulnerabilityTime);
            } else
            {
                _health = new NonPersistentAttribute();
                _invulnerabilityTime = new NonPersistentAttribute();
            }
        }

        private void OnHitTaken(ref EntityEventContext ctx)
        {
            if (!_isInvulnerable)
            {
                _health.CurrentValue -= ctx.Damage.Damage;

                if (_health.CurrentValue <= 0)
                {
                    _eventController.TriggerEvent(EntityEventType.OnDeath, ctx);
                }

                StartCoroutine(InvulnerabilityTimeCoroutine());
            }
        }

        private IEnumerator InvulnerabilityTimeCoroutine()
        {
            _isInvulnerable = true;

            yield return new WaitForSeconds(_invulnerabilityTime.CurrentValue);

            _isInvulnerable = false;
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
