using Game.Entities.Shared;
using Game.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerMovementController : MovementController, IEventListener
    {
        private NonPersistentAttribute _sprintingMultiplier;
        private NonPersistentAttribute _staminaLoss;
        private NonPersistentAttribute _stamina;
        private NonPersistentAttribute _staminaRecoveryDelay;
        private NonPersistentAttribute _staminaRecoveryPercentage;
        private EntityEventController _eventController;
        
        private bool _isSprinting = false;

        private Coroutine _sprintCoroutine;
        private Coroutine _staminaRecoveryCoroutine;
        
        [SerializeField]
        private SprintFOV _sprintFOV;

        protected override void Awake()
        {
            base.Awake();

            _eventController = GetComponent<EntityEventController>();
        }

        protected override void SetNonPersistentAttributes()
        {
            base.SetNonPersistentAttributes();

            if (_attributeController)
            {
                _sprintingMultiplier = _attributeController.GetNonPersistentAttribute(AttributeType.SprintMultiplier);
                _stamina = _attributeController.GetNonPersistentAttribute(AttributeType.Stamina);
                _staminaLoss = _attributeController.GetNonPersistentAttribute(AttributeType.StaminaLoss);
                _staminaRecoveryDelay = _attributeController.GetNonPersistentAttribute(AttributeType.StaminaRecoveryDelay);
                _staminaRecoveryPercentage = _attributeController.GetNonPersistentAttribute(AttributeType.StaminaRecoveryPercentage);
            } else
            {
                _sprintingMultiplier = new NonPersistentAttribute();
                _stamina = new NonPersistentAttribute();
                _staminaLoss = new NonPersistentAttribute();
                _staminaRecoveryDelay = new NonPersistentAttribute();
                _staminaRecoveryPercentage = new NonPersistentAttribute();
            }
        }

        protected override Vector3 GetMovementForce(Vector3 direction)
        {
            if (_isSprinting)
            {
                return base.GetMovementForce(direction) * _sprintingMultiplier.CurrentValue;
            } else
            {
                return base.GetMovementForce(direction);
            }           
        }

        private IEnumerator SprintCoroutine()
        {
            while (CanSprint() && _isSprinting)
            {
                _stamina.CurrentValue -= Time.deltaTime * _staminaLoss.CurrentValue;

                _eventController.TriggerEvent(EntityEventType.OnStaminaChange, new EntityEventContext() { StaminaChange = new EntityEventContext.StaminaChangePacket() { CurrentStamina = _stamina.CurrentValue, MaxStamina = _stamina.MaxValue } });

                yield return new WaitForFixedUpdate();
            }

            StopSprinting();
        }

        private bool CanSprint()
        {
            return _isGrounded && _stamina.CurrentValue > 0;
        }

        private void StartSprinting()
        {
            if (CanSprint())
            {
                _isSprinting = true;

                if (_staminaRecoveryCoroutine != null)
                {
                    StopCoroutine(_staminaRecoveryCoroutine);
                    _staminaRecoveryCoroutine = null;
                }

                if (_sprintCoroutine != null)
                {
                    StopCoroutine(_sprintCoroutine);
                }

                _sprintFOV.StartSprinting();
                _sprintCoroutine = StartCoroutine(SprintCoroutine());
            } else if (_isSprinting)
            {
                StopSprinting();
            }
        }

        private void StopSprinting()
        {
            _sprintFOV.StopSprinting();

            _isSprinting = false;

            if (_staminaRecoveryCoroutine != null)
            {
                StopCoroutine(_staminaRecoveryCoroutine);
            }

            _staminaRecoveryCoroutine = StartCoroutine(StaminaRecoveryCoroutine());
        }

        private IEnumerator StaminaRecoveryCoroutine()
        {
            yield return new WaitForSeconds(_staminaRecoveryDelay.CurrentValue);

            float step = _staminaRecoveryPercentage.CurrentValue * _stamina.MaxValue / 10;

            while (_stamina.CurrentValue < _stamina.MaxValue)
            {
                _stamina.CurrentValue += step;

                _eventController.TriggerEvent(EntityEventType.OnStaminaChange, new EntityEventContext() { StaminaChange = new EntityEventContext.StaminaChangePacket() { CurrentStamina = _stamina.CurrentValue, MaxStamina = _stamina.MaxValue } });

                yield return new WaitForFixedUpdate();
            }

            _staminaRecoveryCoroutine = null;
        }

        private void OnSprintButtonEnded(ref EntityEventContext ctx)
        {
            if (_isSprinting)
            {
                StopSprinting();
            }
        }

        private void OnSprintButtonStarted(ref EntityEventContext ctx)
        {
            StartSprinting();
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnSprintButtonStarted, OnSprintButtonStarted);
                _eventController.AddListener(EntityEventType.OnSprintButtonEnded, OnSprintButtonEnded);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnSprintButtonStarted, OnSprintButtonStarted);
                _eventController.RemoveListener(EntityEventType.OnSprintButtonEnded, OnSprintButtonEnded);
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
    }
}
