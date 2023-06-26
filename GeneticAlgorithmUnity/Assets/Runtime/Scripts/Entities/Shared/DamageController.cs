using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    /// <summary>
    /// Controls all damage that the entity does to another entity.
    /// </summary>
    public class DamageController : MonoBehaviour, IEventListener
    {
        private EntityEventController _eventController;
        
        private NonPersistentAttribute _allDamageMultiplier;
        private NonPersistentAttribute _explosiveDamageMultiplier;

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
                _allDamageMultiplier = _attributeController.GetNonPersistentAttribute(AttributeType.AllDamageMultiplier);
                _explosiveDamageMultiplier = _attributeController.GetNonPersistentAttribute(AttributeType.ExplosiveDamageMultiplier);
            }
            else
            {
                _allDamageMultiplier = new NonPersistentAttribute();
                _explosiveDamageMultiplier = new NonPersistentAttribute();
            }
        }

        private void OnHitDealt(ref EntityEventContext ctx)
        {
            if (ctx.Other != null && ctx.Damage != null && ctx.Damage.DamageType != Events.DamageType.None)
            {
                ctx.Damage.Damage += ctx.Damage.Damage * _allDamageMultiplier.CurrentValue + ctx.Damage.Damage * _explosiveDamageMultiplier.CurrentValue;

                if (ctx.Damage.FriendlyFire)
                {
                    ctx.Damage.Damage *= 0.1f;
                }

                EntityEventContext otherCtx = new EntityEventContext()
                {
                    Owner = ctx.Other,
                    Other = ctx.Owner,
                    Damage = ctx.Damage
                };

                ctx.Other.GetComponent<EntityEventController>().TriggerEvent(EntityEventType.OnHealthChange, otherCtx);
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
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnHitDealt, OnHitDealt);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnHitDealt, OnHitDealt);
            }
        }
    } 
}
