using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class HealingController : MonoBehaviour, IEventListener
    {
        private EntityEventController _eventController;

        private NonPersistentAttribute _attribute1;
        private NonPersistentAttribute _attribute2;

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
                _attribute1 = _attributeController.GetNonPersistentAttribute(AttributeType.Health);
                _attribute2 = _attributeController.GetNonPersistentAttribute(AttributeType.InvulnerabilityTime);
            }
            else
            {
                _attribute1 = new NonPersistentAttribute();
                _attribute2 = new NonPersistentAttribute();
            }
        }

        private void OnHitDealt(ref EntityEventContext ctx)
        {
            if (ctx.Other != null && ctx.Healing != null && ctx.Healing.HealingType != HealingType.None)
            {
                EntityEventContext otherCtx = new EntityEventContext()
                {
                    Owner = ctx.Other,
                    Other = ctx.Owner,
                    Healing = ctx.Healing
                };

                ctx.Other.GetComponent<EntityEventController>().TriggerEvent(EntityEventType.OnHealtChange, otherCtx);
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
