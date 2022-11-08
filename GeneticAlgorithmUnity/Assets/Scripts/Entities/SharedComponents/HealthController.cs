using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System;

namespace Game.Entities
{
    public class HealthController : MonoBehaviour, IEventListener, IModifyAttribute
    {
        [SerializeField]
        private Health _health;

        private EntityEventController _eventController;

        private float _maxHealth;
        [SerializeField]
        private float _currentHealth;

        public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
        public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            CurrentHealth = _health.CurrentValue;
        }

        private void OnHitTaken(ref EntityEventContext ctx)
        {
            _health.ModifyValue(ref _currentHealth, ctx.healthModifier);

            if (_currentHealth <= 0)
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
                _eventController.AddListener(EntityEventType.OnTest, OnTest);
            }
        }

        private void OnTest(ref EntityEventContext ctx)
        {
            Debug.Log("Take damage: " + ctx.healthModifier);
        }

        public void StopListening()
        {
            if (_eventController != null)
            {
                _eventController.RemoveListener(EntityEventType.OnHitTaken, OnHitTaken);
            }
        }

        public float GetCurrentValue()
        {
            return _currentHealth;
        }

        public float GetMaximumValue()
        {
            return _maxHealth;
        }

        public void ModifyMaximumValue(float change)
        {
            _health.ModifyValue(ref _maxHealth, change);
        }

        public void ModifyCurrentValue(float change)
        {
            _health.ModifyValue(ref _currentHealth, change);
        }

        public void ModifyMinimumValue(float changeAmount)
        {
            return;
        }

        public float GetMinimumValue()
        {
            return 0f;
        }
    } 
}
