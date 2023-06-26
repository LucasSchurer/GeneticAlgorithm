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
        private NonPersistentAttribute _damageReduction;

        private EntityEventController _eventController;
        private AttributeController _attributeController;

        private bool _isInvulnerable = false;

        protected virtual void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _attributeController = GetComponent<AttributeController>();
        }

        protected virtual void Start()
        {
            if (_attributeController)
            {
                _health = _attributeController.GetNonPersistentAttribute(AttributeType.Health);
                _invulnerabilityTime = _attributeController.GetNonPersistentAttribute(AttributeType.InvulnerabilityTime);
                _damageReduction = _attributeController.GetNonPersistentAttribute(AttributeType.DamageReduction);
            } else
            {
                _health = new NonPersistentAttribute();
                _invulnerabilityTime = new NonPersistentAttribute();
                _damageReduction = new NonPersistentAttribute();
            }
        }

        private void OnHealthChange(ref EntityEventContext ctx)
        {
            if (ctx.Healing != null && ctx.Healing.HealingType != HealingType.None)
            {
                Heal(ref ctx);
            }

            if (ctx.Damage != null && ctx.Damage.DamageType != Events.DamageType.None)
            {
                Damage(ref ctx);
            }
        }

        private void Heal(ref EntityEventContext ctx)
        {
            if (ctx.Healing.Healing > 0f)
            {
                _health.CurrentValue += ctx.Healing.Healing;

                ctx.HealthChange = new EntityEventContext.HealthChangePacket() { MaxHealth = _health.MaxValue, CurrentHealth = _health.CurrentValue };

                _eventController.TriggerEvent(EntityEventType.OnHealingTaken, ctx);
                
                if (ctx.Other != null)
                {
                    ctx.Other.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnHealingDealt, ctx);
                }

                Healed();
            }
        }

        protected virtual void Healed() { }

        private void Damage(ref EntityEventContext ctx)
        {
            if (!_isInvulnerable)
            {
                if (ctx.Damage.Damage > 0f)
                {
                    _health.CurrentValue -= ctx.Damage.Damage - (ctx.Damage.Damage * _damageReduction.CurrentValue);

                    if (_health.CurrentValue <= 0)
                    {
                        _eventController.TriggerEvent(EntityEventType.OnDeath, ctx);
                    }

                    ctx.HealthChange = new EntityEventContext.HealthChangePacket() { MaxHealth = _health.MaxValue, CurrentHealth = _health.CurrentValue };

                    _eventController.TriggerEvent(EntityEventType.OnDamageTaken, ctx);

                    if (ctx.Other != null)
                    {
                        ctx.Other.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnDamageDealt, ctx);
                    }

                    Damaged();

                    StartCoroutine(InvulnerabilityTimeCoroutine());
                }
            }
        }

        protected virtual void Damaged() { }

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
                _eventController.AddListener(EntityEventType.OnHealthChange, OnHealthChange);
            }
        }

        public void StopListening()
        {
            if (_eventController != null)
            {
                _eventController.RemoveListener(EntityEventType.OnHealthChange, OnHealthChange);
            }
        }
    } 
}
