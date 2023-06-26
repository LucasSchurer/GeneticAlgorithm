using Game.Entities;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Enemy
{
    public class BotLookTowards : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private AttributeController _attributeController;
        private NonPersistentAttribute _rotationSpeed;

        [SerializeField]
        private EntityEventController _eventController;

        public Transform Target { get => _target; set => _target = value; }

        private Vector3 DirectionToTarget => (_target == null || transform == null) ? Vector3.zero : (_target.position - transform.position).normalized;

        private void Start()
        {
            if (_attributeController)
            {
                _rotationSpeed = _attributeController.GetNonPersistentAttribute(AttributeType.RotationSpeed);
            }
            else
            {
                _rotationSpeed = new NonPersistentAttribute();
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void FixedUpdate()
        {
            if (DirectionToTarget == Vector3.zero) return;

            Quaternion smoothRotation = Quaternion.LookRotation(DirectionToTarget);

            smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * _rotationSpeed.CurrentValue);

            transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
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
                _eventController.AddListener(EntityEventType.OnDeath, OnDeath, EventExecutionOrder.Before);
            }
        }

        private void OnDeath(ref EntityEventContext ctx)
        {
            enabled = false;
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnDeath, OnDeath, EventExecutionOrder.Before);
            }
        }
    } 
}
