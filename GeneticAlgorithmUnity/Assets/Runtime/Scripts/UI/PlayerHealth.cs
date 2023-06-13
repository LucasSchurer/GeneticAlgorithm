using Game.Entities;
using Game.Events;
using Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    namespace Game.UI
    {
        public class PlayerHealth : MonoBehaviour, IEventListener
        {
            [SerializeField]
            private Slider _bar;
            [SerializeField]
            private TextMeshProUGUI _value;

            private float _currentHealth = -1f;
            private float _maxHealth = -1f;

            [SerializeField]
            private EntityEventController _playerEventController;
            [SerializeField]
            private AttributeController _playerAttributeController;
            
            private void Start()
            {
                if (_playerAttributeController != null)
                {
                    NonPersistentAttribute health = _playerAttributeController.GetNonPersistentAttribute(AttributeType.Health);
                    UpdateHealth(health.CurrentValue, health.MaxValue);
                }
            }

            private void OnPlayerHealthChange(ref EntityEventContext ctx)
            {
                if (ctx.HealthChange != null)
                {
                    UpdateHealth(ctx.HealthChange.CurrentHealth, ctx.HealthChange.MaxHealth);
                }
            }

            private void UpdateHealth(float currentHealth, float maxHealth)
            {
                _currentHealth = currentHealth;
                _maxHealth = maxHealth;
                
                _bar.maxValue = _maxHealth;
                _bar.value = _currentHealth;

                _value.text = $"{_currentHealth} / {_maxHealth}";
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
                if (_playerEventController)
                {
                    _playerEventController.AddListener(EntityEventType.OnHealthChange, OnPlayerHealthChange, EventExecutionOrder.After);
                }
            }

            public void StopListening()
            {
                if (_playerEventController)
                {
                    _playerEventController.RemoveListener(EntityEventType.OnHealthChange, OnPlayerHealthChange, EventExecutionOrder.After);
                }
            }
        }  
    }
}
