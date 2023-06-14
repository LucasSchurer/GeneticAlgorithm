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
        public class PlayerStamina : MonoBehaviour, IEventListener
        {
            [SerializeField]
            private Slider _bar;
            [SerializeField]
            private TextMeshProUGUI _value;

            private float _currentStamina = -1f;
            private float _maxStamina = -1f;

            [SerializeField]
            private EntityEventController _playerEventController;
            [SerializeField]
            private AttributeController _playerAttributeController;

            private void Start()
            {
                if (_playerAttributeController != null)
                {
                    NonPersistentAttribute stamina = _playerAttributeController.GetNonPersistentAttribute(AttributeType.Stamina);
                    UpdateStamina(stamina.CurrentValue, stamina.MaxValue);
                }
            }

            private void OnPlayerStaminaChange(ref EntityEventContext ctx)
            {
                if (ctx.StaminaChange != null)
                {
                    UpdateStamina(ctx.StaminaChange.CurrentStamina, ctx.StaminaChange.MaxStamina);
                }
            }

            private void UpdateStamina(float currentHealth, float maxHealth)
            {
                _currentStamina = currentHealth;
                _maxStamina = maxHealth;

                _bar.maxValue = _maxStamina;
                _bar.value = _currentStamina;

                _value.text = $"{_currentStamina.ToString("#.#")} / {_maxStamina.ToString("#.#")}";
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
                    _playerEventController.AddListener(EntityEventType.OnStaminaChange, OnPlayerStaminaChange, EventExecutionOrder.After);
                }
            }

            public void StopListening()
            {
                if (_playerEventController)
                {
                    _playerEventController.RemoveListener(EntityEventType.OnStaminaChange, OnPlayerStaminaChange, EventExecutionOrder.After);
                }
            }
        }
    }
}
